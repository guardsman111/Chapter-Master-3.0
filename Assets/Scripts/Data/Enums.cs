using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChapterMaster.Data
{
    public class Enums
    {
        public enum SquadType
        {
            Tactical,
            Assault,
            Devastator,
            Elite,
            HQ
        }

        public enum SoldierDesignation
        {
            Line,
            Close,
            Heavy,
            Elite,
            HQ
        }

        public enum WeaponType
        {
            Primary,
            Secondary,
            Melee,
            VehiclePrimary,
            VehicleSecondary,
            VehicleMelee
        }

        public enum ArmourType
        {
            Carapace,
            Power,
            Terminator,
            Vehicle
        }
    }
}
