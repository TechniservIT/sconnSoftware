﻿using System;

using WatchKit;
using Foundation;

namespace sconnRem.Watch.WatchKitExtension
{
    public partial class NotificationController : WKUserNotificationInterfaceController
    {
        public NotificationController(IntPtr handle) : base(handle)
        {
        }

        public override void Awake(NSObject context)
        {
            base.Awake(context);

            // Configure interface objects here.
            Console.WriteLine("{0} awake with context", this);
        }

        public override void WillActivate()
        {
            // This method is called when the watch view controller is about to be visible to the user.
            Console.WriteLine("{0} will activate", this);
        }

        public override void DidDeactivate()
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);
        }

        public override void DidReceiveLocalNotification(UIKit.UILocalNotification localNotification, Action<WKUserNotificationInterfaceType> completionHandler)
        {
            // This method is called when a local notification needs to be presented.
            // Implement it if you use a dynamic glance interface.
            // Populate your dynamic glance inteface as quickly as possible.
            //
            // After populating your dynamic glance interface call the completion block.
            completionHandler(WKUserNotificationInterfaceType.Custom);
        }

        public override void DidReceiveRemoteNotification(NSDictionary remoteNotification, Action<WKUserNotificationInterfaceType> completionHandler)
        {
            // This method is called when a remote notification needs to be presented.
            // Implement it if you use a dynamic glance interface.
            // Populate your dynamic glance inteface as quickly as possible.
            //
            // After populating your dynamic glance interface call the completion block.
            completionHandler(WKUserNotificationInterfaceType.Custom);

            // Use the following constant to display the static notification.
            //completionHandler(WKUserNotificationInterfaceType.Default);
        }
    }
}

