using System;
using System.Threading.Tasks;
using Foundation;
using Newtonsoft.Json;
using UIKit;
using OfflineSyncSample.Manager.Sync;
using OfflineSyncSample.Manager.Notification;
using static OfflineSyncSample.Utiliies.Enums;
using Xamarin.Essentials;

namespace OfflineSyncSample
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate
    {
        private SyncManager syncManager = null;
        private NotificationManager notificationManager = null;

        public SyncManager SyncManager
        {
            get
            {
                return syncManager;
            }
        }

        public NotificationManager NotificationManager
        {
            get
            {
                return notificationManager;
            }
        }

        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            //Register Meathod 1 - Background fetch
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            //Register Meathod 2 - Background processing
            if (syncManager == null)
            {
                syncManager = new SyncManager();
            }

            if (notificationManager == null)
            {
                notificationManager = new NotificationManager();
            }

            //Register for notification
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );

                application.RegisterUserNotificationSettings(notificationSettings);
            }

            Connectivity.ConnectivityChanged += DidConnectivityChange;

            return true;
        }

        async void DidConnectivityChange(object sender, ConnectivityChangedEventArgs e)
        {
            if(e.NetworkAccess == NetworkAccess.Internet)
            {
                //start a task
                //var result = await this.SyncManager.NewDownloadTask();

                //Get all task
                var tasks = this.syncManager.SyncSession.GetAllTasksAsync().Result;
            }
        }

        [Export("application:performFetchWithCompletionHandler:")]
        public async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            Console.WriteLine("Meathod 1 - Background fetch called...");
            //Check for any Data
            bool hasPendingData = true;
            var result = UIBackgroundFetchResult.NoData;

            if (!hasPendingData)
            {
                completionHandler(result);
                return;
            }

            try
            {
                //TODO: Sync Data to API
                var uploadResult = await DownloadDataSampleMeathod1();
                var downloadResult = await DownloadDataSampleAsyncMeaathod1();

                result = UIBackgroundFetchResult.NewData;
            }
            catch
            {
                result = UIBackgroundFetchResult.Failed;
            }
            finally
            {
                completionHandler(result);
            }
        }

        [Export("application:didReceiveLocalNotification:")]
        public void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            UIAlertController alertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alertController, true, null);

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        private void persistInDB(string DB)
        {
            //TODO: append DB calls here
        }

        private void updateUI()
        {
            /*InvokeOnMainThread(() =>
            {
                this.labelTemp.Text = weatherInfo.GetTempInCelsius().ToString() + "°";
                this.labelLocation.Text = weatherInfo.name;
            });*/

        }

        public async Task<string> DownloadDataSampleAsyncMeaathod1()
        {
            await Task.Delay(2000);

            return "Dummy sucess message";
        }

        public async Task<string> DownloadDataSampleMeathod1()
        {
            await Task.Delay(2000);

            return "Dummy sucess message";
        }



        public void SyncCompleted(NSUrlSessionTask sessionTask)
        {
            try
            {
                if (sessionTask.Response == null || string.IsNullOrEmpty(sessionTask.Response.ToString()))
                {
                    Console.WriteLine("Success, But no response found.");
                }
                else
                {
                    var resp = (NSHttpUrlResponse)sessionTask.Response;
                    var statusCode = resp.StatusCode;
                    var taskId = Convert.ToInt32(sessionTask.TaskIdentifier);

                    if (sessionTask.State == NSUrlSessionTaskState.Completed)
                    {
                        if ((int)statusCode == 200)
                        {
                            InvokeOnMainThread(delegate
                            {
                                this.NotificationManager.ShowUploadNotification(true, "Success For Task ID :" + taskId);
                            });

                            SyncManager.UpdateSyncStatus(taskId, SyncStatus.Completed);
                        }
                        else
                        {
                            InvokeOnMainThread(delegate
                            {
                                this.NotificationManager.ShowUploadNotification(false, "Failed For Task ID :" + taskId);
                            });

                            SyncManager.UpdateSyncStatus(taskId, SyncStatus.Failed);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ProcessCompletedTask Ex: {0}", ex.Message);
            }
        }

        // UISceneSession Lifecycle

        [Export("application:configurationForConnectingSceneSession:options:")]
        public UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            // Called when a new scene session is being created.
            // Use this method to select a configuration to create the new scene with.
            return UISceneConfiguration.Create("Default Configuration", connectingSceneSession.Role);
        }

        [Export("application:didDiscardSceneSessions:")]
        public void DidDiscardSceneSessions(UIApplication application, NSSet<UISceneSession> sceneSessions)
        {
            // Called when the user discards a scene session.
            // If any sessions were discarded while the application was not running, this will be called shortly after `FinishedLaunching`.
            // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        }
    }
}

