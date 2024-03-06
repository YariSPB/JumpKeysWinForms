# JumpKeysWinForms
JumpKeys is a .Net Library providing tools for customizable keyboard navigations for Windows Forms.

Principles of accessible UI are laid out in this paper: https://www.linkedin.com/pulse/unlocking-web-all-building-accessible-online-yaroslav-paslavskiy-th9sf/?trackingId=8dHd3BNjS2qngC5px0a%2FKg%3D%3D

The goal of the project is to improve UI accessibility for people preferring work with a keyboard. Also, a greater goal is to make it WCAG and 508 Compliant.

Download this C# Class Library and apply to your Winforms project.
At form inititation, register navigation setup for Controls.
JumpKeys uses builder pattern and extention methods to customize navigation behavior for a selected Control instances and its nested elements.
For example:

Registering Tab navigation for a MainStrip element, for each Item ("File", "Edit", "View", etc.), while skipping TextBoxes, if any, and only Tab first 3 Items and then jump to another control.

JKSetup.ForMenuStrip(someMainStrip)
.SkipTextBox()
.SkipAfter(3)
.Register();

This is an open source project with a lot of work to be done. 
Feel free to contribute and raise issues.




