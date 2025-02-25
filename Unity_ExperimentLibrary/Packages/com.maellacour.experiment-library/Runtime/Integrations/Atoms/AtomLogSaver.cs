namespace ExperimentLibrary.Integrations.Atoms
{

#if UNITY_ATOMS  // Only compiles if Unity Atoms package is present
    using UnityEngine;
    using UnityAtoms.BaseAtoms;
    using ExperimentLibrary;

    /// <summary>
    /// Implementation of LogSaver that uses Unity Atoms IntReference for the participant ID.
    /// </summary>
    public class AtomLogSaver : LogSaver
    {
        [SerializeField]
        private IntReference _participantIdReference;

        /// <inheritdoc/>
        protected override void SaveLogsIfNeeded()
        {
            if (!_hasQuit && _participantIdReference != null)
            {
                SaveLogs(_participantIdReference.Value);
            }
        }
    }
#endif

}