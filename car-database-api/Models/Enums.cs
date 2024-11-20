namespace car_database_api.Models;

// public enum GearboxType
// {
//     Automatic,
//     Manual
// }
//
// public enum FuelType
// {
//     Petrol,
//     Diesel,
//     Hybrid,
//     Electric,
//     Hydrogen
// }

public enum CarType
{
    compact,
    economy,
    van,
    suv
}

public enum RentalStatus
{
    planned,
    inProgress,
    pendingReturn,
    ended
}