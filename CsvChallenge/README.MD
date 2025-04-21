CsvChallenge
This project analyzes rainfall data and provides insights such as average rainfall, device status, and rainfall trends.


Assumptions
Data Are Not Sorted by Time:

The rainfall data provided in the CSV files are not guaranteed to be sorted by time. The program handles this internally.
Trend Calculation:

For calculating rainfall trends, only the current window (e.g., the last 4 hours) and the previous window are considered.
Device Status Logic:

A device is marked as Red if any rainfall reading in the last 4 hours exceeds 30mm, regardless of the average rainfall.