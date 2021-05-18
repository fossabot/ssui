﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SolidShineUi.Keyboard
{
    public class RoutedEventKeyAction : IKeyAction
    {
#if NETCOREAPP
        RoutedEventHandler? reh = null;
        public UIElement? SourceElement { get; private set; }

        public RoutedEventKeyAction(RoutedEventHandler reh, string methodId, UIElement? sourceElement = null)
#else
        RoutedEventHandler reh = null;
        public UIElement SourceElement { get; private set; }

        public RoutedEventKeyAction(RoutedEventHandler reh, string methodId, UIElement sourceElement = null)
#endif
        {
            ID = methodId;
            this.reh = reh;
            SourceElement = sourceElement;
        }

        public string ID { get; set; }

        public void Execute()
        {
            if (reh != null)
            {
#if NETCOREAPP
                reh.Invoke((object?)SourceElement ?? this, new RoutedEventArgs(UIElement.KeyDownEvent, this));
#else
                reh.Invoke((object)SourceElement ?? this, new RoutedEventArgs(UIElement.KeyDownEvent, this));
#endif
            }
        }

        public static KeyActionList CreateListFromMenu(Menu m)
        {
            KeyActionList rekya = new KeyActionList();

#if NETCOREAPP
            foreach (object? element in m.Items)
#else
            foreach (object element in m.Items)
#endif
            {
                if (element is MenuItem mi && element != null)
                {
                    if (mi.Items.Count > 0)
                    {
                        rekya.AddRange(CreateListFromMenuItem(mi));
                    }
                    else
                    {
                        // if this a top-level MenuItem that has a click event of its own, then okay
                        try
                        {
                            rekya.Add(CreateFromMenuItem(mi));
                        }
                        catch (MissingMemberException) { }
                        catch (InvalidOperationException) { }
                    }
                }
            }

            return rekya;
        }

        private static List<RoutedEventKeyAction> CreateListFromMenuItem(MenuItem mmi)
        {
            List<RoutedEventKeyAction> rekya = new List<RoutedEventKeyAction>();

#if NETCOREAPP
            foreach (object? item in mmi.Items)
#else
            foreach (object item in mmi.Items)
#endif
            {
                if (item is MenuItem mi && item != null)
                {
                    try
                    {
                        rekya.Add(CreateFromMenuItem(mi));
                    }
                    catch (MissingMemberException) { }
                    catch (InvalidOperationException) { }

                    if (mi.Items.Count > 0)
                    {
                        rekya.AddRange(CreateListFromMenuItem(mi));
                    }
                }
            }

            return rekya;
        }

        /// <summary>
        /// Generate a RoutedEventKeyAction using the Click event of a MenuItem. If the MenuItem doesn't have the Click event handled, an exception is thrown.
        /// </summary>
        /// <param name="mi">The menu item to use the Click event from.</param>
        /// <exception cref="MissingMethodException">Thrown if the MenuItem does not have any Click event handlers.</exception>
        /// <exception cref="InvalidOperationException">Thrown if there was an issue entering into the MenuItem via reflection. If this error occurs, please contact the library creator.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the MenuItem is null.</exception>
        public static RoutedEventKeyAction CreateFromMenuItem(MenuItem mi)
        {
            if (mi != null)
            {
                try
                {
                    // source: https://stackoverflow.com/questions/982709/removing-routed-event-handlers-through-reflection
                    var eventInfo = mi.GetType().GetEvent("Click", BindingFlags.Public | BindingFlags.FlattenHierarchy);
                    TypeInfo t = mi.GetType().GetTypeInfo();
                    var clickEvent = t.DeclaredFields.Where((ei) => { return ei.Name == "ClickEvent"; }).First();
#if NETCOREAPP
                    RoutedEvent re = (RoutedEvent)clickEvent.GetValue(mi)!;

                    PropertyInfo pi = t.GetProperty("EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic)!;
                    var ehs = pi.GetValue(mi);
                    if (ehs != null)
                    {
                        var delegates = (RoutedEventHandlerInfo[]?)ehs.GetType().GetMethod("GetRoutedEventHandlers")!?.Invoke(ehs, new object[] { MenuItem.ClickEvent });
#else
                    RoutedEvent re = (RoutedEvent)clickEvent.GetValue(mi);

                    PropertyInfo pi = t.GetProperty("EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
                    var ehs = pi.GetValue(mi);
                    if (ehs != null)
                    {
                        var delegates = (RoutedEventHandlerInfo[])ehs.GetType().GetMethod("GetRoutedEventHandlers")?.Invoke(ehs, new object[] { MenuItem.ClickEvent });
#endif
                        if (delegates != null)
                        {
                            if (delegates.Length > 0)
                            {
                                var dele = delegates[0].Handler;
                                var handler = (RoutedEventHandler)delegates[0].Handler;
                                return new RoutedEventKeyAction(handler, mi.Name, mi);
                            }
                            else
                            {
                                throw new MissingMethodException("This menu item has no click event handlers.");
                            }
                        }
                        else
                        {
                            throw new MissingMethodException("This menu item has no click event handlers.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not enter into the menu item to determine any click event handlers it had. Please contact the library creator if you see this error.");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException("Could not enter into the menu item to determine any click event handlers it had. Please contact the library creator if you see this error.", ex);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(mi));
            }
        }
    }
}
