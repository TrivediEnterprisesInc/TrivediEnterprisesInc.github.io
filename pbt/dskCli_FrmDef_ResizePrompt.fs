* Extend tbl 2 cellPnl, draw 3 lines botR on mouseHover
* On mouseDown trigger resize
* During drag show Red lines; > 50px snap to next ctrl in direction (ensure can't go beyond @ borders)
* When op completes frm.doLayout is triggered

@TBD: Port earlier version to feed the bot?


//Resizing / SnapLines / Handles for Ctrls
(*
Note: Be _VERY_CAREFUL_ using the links below, for this reason:
    We had good/sufficient links on the 26th, but the monkeyBastas hacked the pointers to those urls (cleared tiny etc.).  So these are probably ppl connected to the monkeys; and of course they're evil as eff. and trying to steal/make a dime somehow (all they know, can't blame them)... effers.
*)
Resizing Ctrls:
**(best, but uses WndProc) https://stackoverflow.com/questions/17264225/how-can-user-resize-control-at-runtime-in-winforms
https://stackoverflow.com/questions/515346/how-to-trigger-an-control-resize-event-without-actually-resizing
https://stackoverflow.com/questions/31845986/resizable-textbox-in-winforms

Copilot:
To enable runtime resizing of controls in a Windows Forms application using C#, you can implement custom logic to allow users to resize controls dynamically. Below is an example of how you can achieve this:
Example: Enable Runtime Resizing of a Control
using System;
using System.Drawing;
using System.Windows.Forms;

public class ResizableForm : Form
{
    private bool isResizing = false;
    private Point lastMousePosition;
    private Control resizableControl;

    public ResizableForm()
    {
        // Create a sample control (e.g., a Panel)
        resizableControl = new Panel
        {
            BackColor = Color.LightBlue,
            Size = new Size(200, 150),
            Location = new Point(50, 50)
        };

        // Add Mouse events for resizing
        resizableControl.MouseDown += ResizableControl_MouseDown;
        resizableControl.MouseMove += ResizableControl_MouseMove;
        resizableControl.MouseUp += ResizableControl_MouseUp;

        // Add the control to the form
        Controls.Add(resizableControl);
    }

    private void ResizableControl_MouseDown(object sender, MouseEventArgs e)
    {
        // Start resizing if the user clicks near the bottom-right corner
        if (e.Button == MouseButtons.Left && e.X >= resizableControl.Width - 10 && e.Y >= resizableControl.Height - 10)
        {
            isResizing = true;
            lastMousePosition = e.Location;
        }
    }

    private void ResizableControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (isResizing)
        {
            // Calculate the new size based on mouse movement
            int newWidth = resizableControl.Width + (e.X - lastMousePosition.X);
            int newHeight = resizableControl.Height + (e.Y - lastMousePosition.Y);

            // Set the new size, ensuring minimum dimensions
            resizableControl.Size = new Size(Math.Max(50, newWidth), Math.Max(50, newHeight));
        }
        else
        {
            // Change cursor to indicate resizing near the bottom-right corner
            if (e.X >= resizableControl.Width - 10 && e.Y >= resizableControl.Height - 10)
            {
                resizableControl.Cursor = Cursors.SizeNWSE;
            }
            else
            {
                resizableControl.Cursor = Cursors.Default;
            }
        }
    }

    private void ResizableControl_MouseUp(object sender, MouseEventArgs e)
    {
        // Stop resizing
        if (e.Button == MouseButtons.Left)
        {
            isResizing = false;
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new ResizableForm());
    }
}

Explanation

Mouse Events:
MouseDown: Detects when the user starts resizing by clicking near the bottom-right corner.
MouseMove: Dynamically adjusts the size of the control as the mouse moves.
MouseUp: Stops resizing when the mouse button is released.

Cursor Change:
The cursor changes to a resize indicator (Cursors.SizeNWSE) when hovering near the bottom-right corner.

Size Constraints:
The control's size is constrained to a minimum width and height to prevent it from disappearing.

Customization
You can adapt this logic to allow resizing from all edges or corners by checking the mouse position relative to the control's bounds.
This approach provides a simple and effective way to enable runtime resizing of controls in a Windows Forms application.

GrabHandles:
https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.controlpaint.drawgrabhandle?view=windowsdesktop-6.0
https://www.cyotek.com/blog/adding-drag-handles-to-an-imagebox-to-allow-resizing-of-selection-regions

SnapLines:
https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.design.behavior.snapline?view=windowsdesktop-9.0 (see snapline.cs)



