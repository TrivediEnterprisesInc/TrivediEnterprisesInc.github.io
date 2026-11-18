# gfx-grid Web Component — Prototype 0.1

This is the first integration prototype for wrapping the existing Dojo 1.13 / GridX app in a `<gfx-grid>` custom element.

## What is included

- `js/gfx-grid.js`: custom element, Dojo runtime loader, GridX initialization and resize observer.
- `css/gfx-grid.css`: host sizing and component-owned layout styles.
- `css/gfxGridx.css`: the uploaded GFX GridX visual customizations.
- `data/TreeNestedTestData.js`: the uploaded Oria-derived nested test data module.
- `js/`: uploaded supporting form scripts for reference/migration. The prototype does not load these yet.
- `tests/index.html`: a responsive host-page test shell with collapsible surrounding regions and adjustable component height.

## Run it

Serve this folder from a local web server (not `file://`), then open `/tests/index.html`. The prototype loads Dojo/GridX from the existing Oria GitHub Pages build by default.

The customer-page embed is currently:

```html
<div style="width:100%;height:600px">
  <gfx-grid></gfx-grid>
</div>
<script src="/path-to-gfx-grid/js/gfx-grid.js"></script>
```

The host must give the component a definite height (or place it in a layout whose height resolves). Width and height are inherited from the host element.

## Configuration

Optional attributes:

- `dojo-url`: full URL to `dojo.js`.
- `build-root`: root of the Dojo/GridX build. Expected folders: `dojo`, `dijit`, `dojox`, `gridx`.
- `asset-base`: base URL containing this package's `css/` and `data/` folders.

Example:

```html
<gfx-grid
  dojo-url="/assets/dojo/dojo.js"
  build-root="/assets/dojo-build"
  asset-base="/assets/gfx-grid">
</gfx-grid>
```

## Important prototype limitations

1. **This is not yet a complete port of the 734-line demo page.** It recreates the core GridX construction, tree test data, toolbar, row hover tooltip, selection behavior, and event hooks in the component. The legacy form/dialog scripts are retained in `js/` but are not loaded or wired into this first pass.
2. The Create Grid / Destroy Grid test controls are intentionally not part of the component. The grid is created once when the custom element connects.
3. Dojo/Claro/GridX theme styles are global CSS. The component's own sizing and customization stylesheet is scoped, but the upstream theme itself is not fully isolated from the host page. Avoid Shadow DOM for now because the legacy Dojo popup/dialog behavior expects document-level DOM. A later custom build can prefix or otherwise isolate theme selectors.
4. This prototype expects to own the page's Dojo AMD runtime. It deliberately reports an error if another Dojo loader is already present rather than silently reusing an unknown version or creating a conflicting second loader.
5. GridX resize behavior is implemented using `ResizeObserver` and `grid.resize()` when available. This must be verified in the browser against the exact GridX build. If the build needs a different resize call or fixed-height option, adjust after testing.
6. `autoHeight` is set to `false` so GridX can occupy the fixed-height component viewport. This is an intentional change from the test page's `autoHeight: true` and needs visual validation.
7. The original `commonAux.js` includes a development `window.fetch` interception that replaces responses with mock data. It is not loaded by the component; that global override must never ship in the production package.

## Events

The component dispatches bubbling custom events:

- `gfx-ready` after GridX starts.
- `gfx-error` if dependencies or initialization fail.
- `gfx-create`, `gfx-delete`, `gfx-edit`, and `gfx-view-change` for host/application integration.

Example:

```js
document.querySelector('gfx-grid').addEventListener('gfx-edit', event => {
  console.log('Selected record:', event.detail);
});
```

## Next validation steps

- Verify initial rendering at several host sizes.
- Resize the host repeatedly, including while hidden and then shown.
- Verify grid body, virtual scroller, toolbar and pagination all recalculate correctly.
- Check console/network for missing AMD modules or CSS/image paths.
- Once the layout behavior is stable, move the remaining app-specific dialog/form functionality into the component adapter and replace the Oria test data module with your own data source.
