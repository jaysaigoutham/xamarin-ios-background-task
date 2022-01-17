using System;
using Foundation;
using UIKit;
using static OfflineSyncSample.Utiliies.Enums;

namespace OfflineSyncSample.Manager.Sync
{
    public class SyncManagerDelegate : NSUrlSessionDownloadDelegate
    {
        private SyncManager syncManager;
        

        public SyncManager SyncManager
        {
            get
            {
                return syncManager;
            }
            set
            {
                this.syncManager = value;
            }
        }

        public SyncManagerDelegate(SyncManager syncManager)
        {
            this.syncManager = syncManager;
        }

        [Export("URLSessionDidFinishEventsForBackgroundURLSession:")]
        public void DidFinishEventsForBackgroundSession(NSUrlSession session)
        {
            //Called when all background sessions are called.
        }

        public override void DidCompleteWithError(NSUrlSession session, NSUrlSessionTask task, NSError error)
        {
            //Do not call this below meathod
            ///base.DidCompleteWithError(session, task, error);
            AppDelegate appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;

            if (error == null)
            {
                appDelegate.SyncCompleted(task);
            }
            else
            {
                var taskId = Convert.ToInt32(task.TaskIdentifier);
                this.SyncManager.UpdateSyncStatus(taskId, SyncStatus.Failed);

                InvokeOnMainThread(delegate
                {
                    appDelegate.NotificationManager.ShowUploadNotification(false, "Follwing task ID Failed : " + taskId + " Reason :" + error.Description);
                });
            }
        }

        public override void DidSendBodyData(NSUrlSession session, NSUrlSessionTask task, long bytesSent, long totalBytesSent, long totalBytesExpectedToSend)
        {
            //Called for CreateUploadTask 
        }

        public override void DidFinishDownloading(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
        {
            //Called for CreateDownloadTask 
            Console.WriteLine("File has been written to - " + location.AbsoluteUrl);
        }

        public override void DidWriteData(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
        {
            //Called for CreateDownloadTask 
        }
    }
}
