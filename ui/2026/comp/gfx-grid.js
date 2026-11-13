
/*
 * gfx-grid custom element — first integration prototype.
 * Dojo/GridX are loaded by this component; the host page need not load them.
 * Light DOM is intentional: Dojo popups/dialogs and legacy widgets expect
 * document-level DOM. Theme CSS is global; gfx-specific rules are scoped.
 */
(function () {
  'use strict';

  if (customElements.get('gfx-grid')) return;

  const DEFAULTS = {
    dojoUrl: 'https://oria.github.io/gridx/build/dojo/dojo.js',
    buildRoot: 'https://oria.github.io/gridx/build',
    assetBase: new URL('../', document.currentScript && document.currentScript.src || location.href).href.replace(/\/$/, '')
  };

  let runtimePromise;
  let cssPromise;

  function attr(el, name, fallback) {
    return el.getAttribute(name) || fallback;
  }

  function addStylesheet(url) {
    if ([...document.styleSheets].some(s => s.href === url)) return Promise.resolve();
    return new Promise((resolve, reject) => {
      const link = document.createElement('link');
      link.rel = 'stylesheet';
      link.href = url;
      link.onload = resolve;
      link.onerror = () => reject(new Error('Could not load stylesheet: ' + url));
      document.head.appendChild(link);
    });
  }

  function loadCss(buildRoot, assetBase) {
    if (cssPromise) return cssPromise;
    cssPromise = Promise.all([
      addStylesheet(buildRoot + '/dojo/resources/dojo.css'),
      addStylesheet(buildRoot + '/dijit/themes/claro/claro.css'),
      addStylesheet(buildRoot + '/dijit/themes/claro/document.css'),
      addStylesheet(buildRoot + '/gridx/resources/claro/Gridx.css'),
      addStylesheet(assetBase + '/comp/gfx-grid.css'),
      addStylesheet(assetBase + '../2024/gfxGridx.css')
    ]);
    return cssPromise;
  }

  function loadDojo(dojoUrl, buildRoot, assetBase) {
    if (runtimePromise) return runtimePromise;

    if (window.require || window.dojo || window.dijit || window.dojoConfig) {
      return Promise.reject(new Error(
        'A Dojo/AMD loader already exists on this page. This prototype expects to own the Dojo runtime; do not load another Dojo build alongside it.'
      ));
    }

    window.dojoConfig = {
      async: true,
      parseOnLoad: false,
      packages: [
        { name: 'dojo', location: buildRoot + '/dojo' },
        { name: 'dijit', location: buildRoot + '/dijit' },
        { name: 'dojox', location: buildRoot + '/dojox' },
        { name: 'gridx', location: buildRoot + '/gridx' },
        { name: 'gfx', location: assetBase + '/data' }
      ]
    };

    runtimePromise = new Promise((resolve, reject) => {
      const script = document.createElement('script');
      script.src = dojoUrl;
      script.async = true;
      script.onload = () => {
        if (!window.require) return reject(new Error('Dojo loaded but its AMD require() function is missing.'));
        resolve(window.require);
      };
      script.onerror = () => reject(new Error('Could not load Dojo from ' + dojoUrl));
      document.head.appendChild(script);
    });
    return runtimePromise;
  }

  class GfxGrid extends HTMLElement {
    constructor() {
      super();
      this._grid = null;
      this._observer = null;
      this._destroyed = false;
      this._status = null;
      this._mount = null;
      this._resizeQueued = false;
    }

    connectedCallback() {
      if (this._started && !this._destroyed) return;
      this._started = true;
      this._destroyed = false;
      this.classList.add('gfx-grid-host');
      this.innerHTML = '<div class="gfx-grid-root"><div class="gfx-grid-theme claro"><div class="gfx-grid-mount" role="region" aria-label="Data grid"></div></div><div class="gfx-grid-status" aria-live="polite">Loading grid…</div></div>';
      this._mount = this.querySelector('.gfx-grid-mount');
      this._status = this.querySelector('.gfx-grid-status');
      this._observeSize();
      this._boot().catch(err => this._showError(err));
    }

    disconnectedCallback() {
      this._destroyed = true;
      if (this._observer) this._observer.disconnect();
      if (this._grid && !this._grid._destroyed) {
        try { this._grid.destroyRecursive ? this._grid.destroyRecursive() : this._grid.destroy(); }
        catch (e) { console.warn('gfx-grid cleanup:', e); }
      }
      this._grid = null;
    }

    async _boot() {
      const dojoUrl = attr(this, 'dojo-url', DEFAULTS.dojoUrl);
      const buildRoot = attr(this, 'build-root', DEFAULTS.buildRoot).replace(/\/$/, '');
      const assetBase = attr(this, 'asset-base', DEFAULTS.assetBase).replace(/\/$/, '');
      await loadCss(buildRoot, assetBase);
      const require = await loadDojo(dojoUrl, buildRoot, assetBase);
      if (this._destroyed) return;

      require([
        'dojo/_base/Deferred',
        'dijit/Toolbar',
        'dijit/form/Button',
        'dijit/form/Select',
        'dijit/Tooltip',
        'dojo/on',
        'dojo/_base/connect',
        'dojo/mouse',
        'gridx/tests/support/stores/ItemFileWriteStore',
        'gridx/allModules',
        'gridx/Grid',
        'gfx/TreeNestedTestData'
      ], (Deferred, Toolbar, Button, Select, Tooltip, on, connect, mouse, storeFactory, modules, Grid, nestedDataSource) => {
        if (this._destroyed) return;
        try {
          this._api = { Deferred, Toolbar, Button, Select, Tooltip, on, connect, mouse, storeFactory, modules, Grid, nestedDataSource };
          this._createGrid(this._api);
        } catch (err) {
          this._showError(err);
        }
      }, err => this._showError(err));
    }

    _createGrid(api) {
      const { Deferred, Toolbar, Button, Select, Tooltip, on, connect, mouse, storeFactory, modules, Grid, nestedDataSource } = api;
      const store = storeFactory({ dataSource: nestedDataSource, maxLevel: 4, maxChildrenCount: 10 });
      store.hasChildren = (id, item) => item && store.getValues(item, 'children').length;
      store.getChildren = item => {
        const d = new Deferred();
        window.setTimeout(() => d.callback(store.getValues(item, 'children')), 250);
        return d;
      };

      const layout = [
        { id: 'number', name: 'number', field: 'number', width: '10%' },
        { id: 'string', name: 'string', field: 'string', width: '45%' },
        { id: 'date', name: 'date', field: 'date', width: '22%' },
        { id: 'time', name: 'time', field: 'time', width: '15%' },
        { id: 'bool', name: 'bool', field: 'bool', width: '8%' },
        { id: 'id', name: 'id', field: 'id', width: 'auto' },
        { id: 'hasPriorVersions', name: 'hasPriorVersions', field: 'hasPriorVersions', width: 'auto' }
      ];

      const gfxModules = [
        modules.Tree, modules.Filter, modules.FilterBar, modules.Pagination,
        modules.PaginationBar, modules.ColumnResizer, modules.ExtendedSelectRow,
        modules.CellWidget, modules.Edit, modules.IndirectSelectColumn,
        modules.SingleSort, modules.VirtualVScroller, modules.Bar, modules.Persist
      ];

      const toolbar = new Toolbar();
      const addButton = (label, iconClass, handler, disabled) => {
        const b = new Button({ label, iconClass, onClick: handler, disabled: !!disabled });
        toolbar.addChild(b);
        return b;
      };

      const selectedRows = () => this._grid && this._grid.select && this._grid.select.row.getSelected();
      const editHandler = () => {
        const selected = selectedRows() || [];
        if (selected.length !== 1) {
          window.alert('Please select one record before clicking Edit, or double-click the record.');
          return;
        }
        store.fetchItemByIdentity({ identity: selected[0], onItem: item => {
          if (item) this.dispatchEvent(new CustomEvent('gfx-edit', { detail: item, bubbles: true }));
        }});
      };
      addButton('Create', 'gridxCreateIcon', () => this.dispatchEvent(new CustomEvent('gfx-create', { bubbles: true })));
      addButton('Edit', 'gridxEditIcon', editHandler);
      const qEdit = addButton('QEdit', 'gridxQEditIcon', editHandler, true);
      addButton('Delete', 'gridxDeleteIcon', () => this.dispatchEvent(new CustomEvent('gfx-delete', { detail: selectedRows(), bubbles: true })));
      addButton('Expand All', 'gridxExpandAllIcon', () => this._grid && this._grid.tree && this._grid.tree.expandAll());
      addButton('Collapse All', 'gridxCollapseAllIcon', () => this._grid && this._grid.tree && this._grid.tree.collapseAll());

      const viewSelect = new Select({
        name: 'gfxDvSelect', style: 'width:100%;', intermediateChanges: true,
        options: [
          { label: 'By Project\\Module', value: 'Tennessee' },
          { label: 'By Module\\SubModule', value: 'Virginia', selected: true },
          { label: 'By Priority', value: 'Washington' },
          { label: 'By Very Long Title Indeed to test autoWidth', value: 'Floridaa' },
          { label: 'By Quadrant', value: 'Florida' }
        ],
        onChange: value => this.dispatchEvent(new CustomEvent('gfx-view-change', { detail: value, bubbles: true }))
      });

      this._grid = new Grid({
        cacheClass: 'gridx/core/model/cache/Async',
        store,
        structure: layout,
        columnWidthAutoResize: true,
        autoWidth: false,
        autoHeight: false,
        treeNested: true,
        paginationBarSizes: [1, 2, 0],
        modules: gfxModules,
        barTop: [[viewSelect], [toolbar]],
        style: 'width:100%;height:100%;'
      });
      this._grid.placeAt(this._mount);
      this._grid.persist.enabled = true;
      toolbar.startup();
      this._grid.startup();
      this._grid.domNode.style.width = '100%';
      this._grid.domNode.style.height = '100%';

      connect.connect(this._grid.select.row, 'onSelectionChange', selected => qEdit.set('disabled', selected.length < 2));
      connect.connect(this._grid, 'onRowMouseOver', e => {
        store.fetchItemByIdentity({ identity: e.rowId, onItem: item => {
          if (!item) return;
          const rowNode = this._grid.body.getRowNode({ visualIndex: e.visualIndex });
          Tooltip.show('row Time fld: ' + item.time, rowNode);
          on.once(rowNode, mouse.leave, () => Tooltip.hide(rowNode));
        }});
      });
      connect.connect(this._grid, 'onRowDblClick', e => {
        const item = this._grid.model.byId(e.rowId);
        this.dispatchEvent(new CustomEvent('gfx-edit', { detail: item, bubbles: true }));
      });

      this._status.hidden = true;
      this._scheduleResize();
      this.dispatchEvent(new CustomEvent('gfx-ready', { bubbles: true }));
    }

    // Public lifecycle hooks are primarily for the external test harness.
    destroyGrid() {
      if (!this._grid) return;
      try { this._grid.destroyRecursive ? this._grid.destroyRecursive() : this._grid.destroy(); }
      finally { this._grid = null; this._mount.replaceChildren(); }
    }

    createGrid() {
      if (this._grid || !this._api || this._destroyed) return;
      this._createGrid(this._api);
    }

    _observeSize() {
      if (!('ResizeObserver' in window)) return;
      this._observer = new ResizeObserver(() => this._scheduleResize());
      this._observer.observe(this);
    }

    _scheduleResize() {
      if (this._resizeQueued) return;
      this._resizeQueued = true;
      requestAnimationFrame(() => {
        this._resizeQueued = false;
        if (!this._grid || this._destroyed) return;
        const w = this._mount.clientWidth;
        const h = this._mount.clientHeight;
        if (w <= 0 || h <= 0) return;
        // GridX inherits Dojo widget sizing. resize() recalculates internal
        // viewport geometry after the host's dimensions have changed.
        if (typeof this._grid.resize === 'function') this._grid.resize();
        if (this._grid.body && typeof this._grid.body.refresh === 'function') this._grid.body.refresh();
      });
    }

    _showError(err) {
      console.error('[gfx-grid]', err);
      if (this._status) {
        this._status.hidden = false;
        this._status.textContent = 'Grid could not be loaded: ' + (err && err.message || err);
        this._status.classList.add('is-error');
      }
      this.dispatchEvent(new CustomEvent('gfx-error', { detail: err, bubbles: true }));
    }
  }

  customElements.define('gfx-grid', GfxGrid);
})();
