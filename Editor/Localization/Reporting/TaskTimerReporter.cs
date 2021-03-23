namespace UnityEditor.Localization.Reporting
{
    static class TaskReporter
    {
        public static ITaskReporter CreateDefaultReporter()
        {
            #if UNITY_2020_1_OR_NEWER
            return new ProgressReporter();
            #else
            return new ProgressBarReporter();
            #endif
        }
    }
}
