
## Assumptions

#### Data Are Not Sorted by Time

The rainfall data provided in the CSV files are not guaranteed to be sorted by time. The program handles this internally.

#### Trend Calculation

For calculating rainfall trends, only the current window (e.g., the last 4 hours) and the previous(current window-4 hours) window are considered.

#### Device Status Logic

A device is marked as Red if any rainfall reading in the last 4 hours exceeds 30mm, regardless of the average rainfall.


## How to Run the Program

### Prerequisites

* Install the .NET SDK (version 9.0 or later).

### Steps to run the program
1. Clone the repository:
```
git clone https://github.com/SyedeeSimon/CsvChallenge.git
cd CsvChallenge
```
2. Ensure the required data files exist:
Create the directory structure
```
mkdir -p CsvChallenge/Resources/Data/DeviceData
mkdir -p CsvChallenge/Resources/Data/RainfallReadingData
```
3. Add a Devices.csv file in DeviceData with sample data:
```
DeviceId,DeviceName,Location
11271,Gauge 1,Test Location 1
25832,Gauge 2,Test Location 2
```
4. Add one or more rainfall data files (e.g., Data1.csv, Data2.csv) in RainfallReadingData with sample data:
```
DeviceId,Time,Rainfall
11271,2023-04-20T08:00:00,10
11271,2023-04-20T09:00:00,20
25832,2023-04-20T10:00:00,15
```
5. Build the project:
```
dotnet build
```
6. Run the program:
```
dotnet run --project CsvChallenge/CsvChallenge.csproj
```
### Steps to run test
1. Change directory to CsvChallenge/CsvChallenge.Tests
```
cd CsvChallenge.Tests
```
2. Run tests
```
dotnet test
```