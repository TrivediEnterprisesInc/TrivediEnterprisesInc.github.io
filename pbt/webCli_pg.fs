//("(.*?)";"(.*?)";"(.*?)";"(.*?)"\);
//https://trivedienterprisesinc.github.io/ui/2024/themes/theming.html

Many of these are double-applications (ie they refer to an existing var defined above)  They can be safely removed; along with stuff we don't nd 2 provide (doesn't affect our wids)

    let styLi = 
        [("Base color for entire theme";"";"@primary-color: #cfe5fa";"#cfe5fa");
        ("Base color for bar-backgrounds";"";"@secondary-color: #efefef";"#efefef");
        ("Text color for enabled widgets";"";"@text-color: #000";"#000");
        ("Base color for disabled backgrounds and borders";"";"@disabled-color: #d3d3d3";"#d3d3d3");
        ("Error color";"";"@error-color: #d46464";"#d46464");
        ("Container Bkgrnd";"(TitlePn, ContentPn, Inputs) (if changed, adjust selected tab to match)";"@container-background-color:#FFFFFF";"#FFFFFF");
        ("Minor Selected color";"(var arrows and buttons)";"@minor-selected-color: spin(saturate(darken(@primary-color, 6), 19), 0)";"#add8ff");
        ("Base Border color";"Augmented/ used directly by variables to create border colors for var wids";"@base-border-color: spin(desaturate(darken(@primary-color, 29), 44), -1)";"#779ec0");
        ("Unfocused Clickable color";"Bkgrd clr 4 enabled buttons, text inputs";"@unfocused-clickable-color: spin(saturate(lighten(@primary-color, 5), 10), 0)";"#e7f3fe");
        ("Border color";"Border color for (enabled, unhovered) TxtBox, Slider, Accord, BorderCont, TabCont";"@border-color: spin(desaturate(darken(@primary-color, 15), 67), 8)";"#b6bdc8");
        ("Minor Border color";"Color of borders inside widgets: horizontal line in Calendar between weeks, around color swatches in ColorPalette, above Dialog action bar";"@minor-border-color: @disabled-color";"#d3d3d3");
        ("Popup Border color";"(Border for Dlg, Mnu, Ttip) Must also update tooltip.png (the arrow image file) to match";"@popup-border-color: @base-border-color";"#779ec0");
        ("Disabled Border color";"(for disabled/readonly btn, txtbox, etc.)     Must also update tooltip.png (the arrow image file) to match";"@disabled-border-color: @disabled-color";"#d3d3d3")
        ("Disabled Bkgrnd color";"(for btn, txtbox, etc.)";"@disabled-background-color: @secondary-color";"#efefef");
        ("Disabled Text color";"(for disabled/readonly wids)";"@disabled-text-color: darken(@secondary-color, 43)";"#828282");
        ("Unselected Bkgrnd color";"(for unselected/unopened tabBtn, accordPn, TitlPn, MnuItms)";"@unselected-background-color: @secondary-color";"#efefef");
        ("Unselected Text color";"";"@unselected-text-color: darken(@secondary-color, 65)";"#efefef");
        ("Hover Border color";"(txtbx, tabLbl, BrdCont spltr, Cal...)";"@hovered-border-color: @base-border-color";"#779ec0");
        ("Hover Bkgrnd color";"(Btn, MnuBar, AccordPn, Calendar... anything that has a (non-white) color to start with and gets darker on hover)";"@hovered-background-color: @minor-selected-color";"#add8ff");
        ("Hover Txt color";"(Title of select AccPn, Lbl of selTab, hoverdMnuItm, etc.)";"@hovered-text-color: @text-color";"#000");
        ("Pressed Border color";"(During clk on CalDay, Slider UpDn btns, tabBtn, etc.)";"@pressed-border-color:  @base-border-color";"#779ec0");
        ("Pressed Bkgrnd color";"(During clk on Acc/TitlPn titlBar, tabBtn, CalDay, TbarBtn, TreeRow.)";"@pressed-background-color: spin(saturate(darken(@primary-color, 16), 12), 0)";"#7fbffa");
        ("Selected Border color";"(AccPn, tab of nestedTabCont (but plain TabContainer is special)";"@selected-border-color: @base-border-color";"#779ec0");
        ("Selected Bkgrnd color";"(AccPn, nestedTabLbl, TreeRow)";"@selected-background-color: @primary-color";"#cfe5fa");
        ("Selected Title color";"(AccPn, selTabLbl, hovered mnuItm)";"@selected-text-color: @text-color";"#000");
        ("Bar Bkgrnd color";"(MenuBar, Toolbar, dlgPnActionBar)";"@bar-background-color: @secondary-color";"#efefef");
        ("Pane Bkgrnd color";"(AccPn, dlg, etc.)";"@pane-background-color: @container-background-color";"#FFFFFF");
        ("Dlg Bkgrnd color";"(MenuBar, Toolbar, dlgPnActionBar)";"@popup-background-color: @container-background-color";"#FFFFFF")]
