using UnityEngine;
using static GenericConst.Const;

namespace MachineData
{
    public enum MachineStatus
    {
        None = DefaultCode,
        Active,
        Inactive
    }

    public static class MachineStatusParser
    {
        public static MachineStatus Parse(string status)
        {
            if (string.IsNullOrEmpty(status))
                return MachineStatus.Inactive;

            switch (status.Trim().ToLowerInvariant())
            {
                case "active":
                    return MachineStatus.Active;
                case "inactive":
                    return MachineStatus.Inactive;
                default:
                    Debug.LogWarning($"[GenericEnum] Unknown status \"{status}\". Defaulting to Inactive.");
                    return MachineStatus.Inactive;
            }
        }
    }
}
