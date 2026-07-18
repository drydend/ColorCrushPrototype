using Turrels.Models;

namespace Turrels.Extensions {
    public static class TurretSpotExtensions {
        public static bool IsEmpty(this TurretSpotModel model) => model.turret == null;
    }
}