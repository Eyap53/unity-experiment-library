namespace ExperimentLibrary
{
    using UnityEngine;

    /// <summary>
    /// Basic implementation of LogSaver that uses a serialized participant ID.
    /// </summary>
    public class SimpleLogSaver : LogSaver
    {
        [SerializeField]
        private int participantId = -1;

        /// <inheritdoc/>
        protected override void SaveLogsIfNeeded()
        {
            if (!_hasQuit && participantId > 0)
            {
                SaveLogs(participantId);
            }
        }
    }
}
