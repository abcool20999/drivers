# driveSync App

Welcome to the driveSync App! This .NET application is designed to facilitate cheaper rides for users by allowing them to share rides with other passengers and inventory items they plan to carry along the way. It includes several controllers and features to manage passengers, drivers, rides, inventory, and bookings.

## Controllers

### PassengerController
- Manages passengers and their details.
- Allows for CRUD operations on passengers.
- Powers the ability to create, read, update, and delete passengers.

### DriverController
- Manages drivers and their details.
- Allows for CRUD operations on drivers.
- Provides validation for driver usernames and passwords.

### RidesController
- Handles trips and trip-related operations.
- Allows for creating, reading, and deleting trips.
- Includes a search feature for the dashboard to enable passengers to search for trips.

### BookingsController
- Manages bookings between passengers and drivers.
- Enables passengers to create, read, and update bookings.
- Shows bookings for drivers and passengers.

## Additional Features

- **Search Feature**: Allows admin to search through a list of users.
- **Date Picker**: Allows users and drivers to select a date.

## Usage

To run the driveSync App locally:

1. Clone this repository.
2. Open the solution file in Visual Studio.
3. Configure the database connection in the appropriate configuration file.
4. Build the solution.
5. Start the application.
6. Access the app through the specified URL in your browser.

## Contributors

1. **Fazhrul Sadip**
   - Models: `Passenger.cs`, `Inventory.cs`
   - Controllers: `PassengerController`, `PassengerDataController`, `InventoryController`, `InventoryDataController`
   - Views: `Passenger`, `Inventory`
   
2. **Awotunde Abraham**
   - Models: `Driver.cs`, `Ride.cs`, `Booking.cs`, `Admin`
   - Controllers: `DriverController`, `DriverDataController`, `RideController`, `RideDataController`, `AdminController`, `AdminDataController`, `BookingController`, `BookingDataController`
   - Views: `Ride`, `Driver`, `Admin`, `Booking`
