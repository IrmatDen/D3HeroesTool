using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace D3HeroesTool.Extensions
{
    public static class ComboBoxExtensionMethods
    {
        /// <summary>
        /// This will allow cb's Width to Match the Width of its widest element.
        /// It does so by triggering these 2 events once:
        /// <list type="bullet">
        /// <item>ComboBox.ItemContainerGenerator.StatusChanged</item>
        /// <item>ComboBox.DropDownOpened</item>
        /// </list>
        /// </summary>
        /// <param name="cb"></param>
        public static void SetWidthFromItems(this ComboBox cb, bool restrictMinWidth)
        {
            double cbWidth = 19; // combobox dropdown button?

            // Get manipulators
            ComboBoxAutomationPeer peer = new ComboBoxAutomationPeer(cb);
            IExpandCollapseProvider provider = (IExpandCollapseProvider)peer.GetPattern(PatternInterface.ExpandCollapse);

            // Create an event that will be called once at the end of the function
            EventHandler evtHandler = null;
            evtHandler = new EventHandler(
                (object o, EventArgs args) =>
                {
                    if (cb.IsDropDownOpen &&
                        cb.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                    {
                        double biggestItemWidth = 0;
                        foreach (var item in cb.Items)
                        {
                            // Get item size by allowing it to expand as much as it wants
                            ComboBoxItem cbItem = cb.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItem;
                            cbItem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            if (cbItem.DesiredSize.Width > biggestItemWidth)
                                biggestItemWidth = cbItem.DesiredSize.Width;
                        }

                        double desiredWith = cbWidth + biggestItemWidth;
                        if (cb.Width < desiredWith)
                            cb.Width = desiredWith;
                        if (restrictMinWidth)
                            cb.MinWidth = desiredWith;

                        cb.ItemContainerGenerator.StatusChanged -= evtHandler;
                        cb.DropDownOpened -= evtHandler;
                        provider.Collapse();
                    }
                });

            cb.ItemContainerGenerator.StatusChanged += evtHandler;
            cb.DropDownOpened += evtHandler;

            // Trigger our sizer event
            provider.Expand();
        }
    }
}
