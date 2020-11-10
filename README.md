## Device monitoring

### Command line utility for monitoring the status of a selected Windows device.

#### Arguments:

 - **-f** - Output file, default *data[date time].csv* next to exe file
 - **-d** - Interval between checks, default: 100 ms
 - **-b** - File write size, default: 100 checks

#### Output file content example:

| Date | State (0 - working, 1 - disabled) |
|--|--|
| 2020-11-10T19:32:06.1315815 | 0 |
| 2020-11-10T19:32:07.2940804 | 0 |
| 2020-11-10T19:32:09.7547642 | 0 |
| 2020-11-10T19:32:11.4857750 | 0 |
| 2020-11-10T19:32:15.2491798 | 1 |
| 2020-11-10T19:32:18.7115560 | 0 |
| 2020-11-10T19:32:21.0036836 | 0 |
| 2020-11-10T19:32:25.7533275 | 0 |

#### An example of a chart built on the output file:
[![enter image description here](https://raw.githubusercontent.com/verloka/DeviceMonitoring/master/merch/chart.png)](https://github.com/verloka/DeviceMonitoring)


## .Net Core 3.1
