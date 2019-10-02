# AI For Good
Proctecting human casuality and accidents in any workplace safety is very important and critical. This is also part of AI for Good projects.

# WorkplaceSafety
AI based - Custom Vision Workplace Safety detection system.

# Use Case

Ability to detect vision-based compliance. In a environment where safety is important for workforce to work there are Compliance rules in place to wear proper gear to safe guard the environment and keep the humans working safe and secured. For example, depending on what manufacturing company the human causality is one thing every one wants to avoid. So, they insist on wearing proper vest, hard hats and safety glass or lab coats and glass to protect. In some cases, may be mask and complete covered suits for chemical spills etc. So, the idea here is to detect human and then see if they are wearing Vest, Hard hats and Safety glass. Usually in manufacturing there are lines where folks can walk and that would be the next future work. Ability to detect humans and compliance and alert management with reporting and Realtime alerts. Also, ability to detect forklift and alert humans on the way to the fork lift operator. The system just not only detect the objects but also ability to store the information for further reporting and analysis.

For example: usually the plants has to provide yearly or quarterly report to OSHA auditors to make sure if there is human causality and what actions were taken not repeat it. Having the picture when the objects were detected and when not detected is super helpful to analyze the data by the auditors. That makes it very simple and easy for auditors. But the main purpose is the plant or factory can be running and there is no downtime or pull work force to go through the auditing process. Usually there is a down time when auditing happens which in this case will reduce and increase productivity and uptime.

It is necessary to detect and provide a report and also it is important to store the data for historical purpose to able to do auditing and also learn from the data. The historical data can be combined with other productivity data and find insights as well. Mostly likely pushing the data in data lake make sense. It is also very important to know how the system is performing. So we need to collect the telemetry and store in Azure SQL and Blob for further processing. We can generate monthly report or weekly report on how many compliance issues were raised. We can also analyze the data and find if the model is performing well or find where is it not.

The scenario can be customized to other use cases like hospitals, chemical plant and various other heavy machinery and mining industries as well.

# Architecture

![alt text](https://github.com/balakreshnan/WorkplaceSafety/blob/master/WorkplaceSafetyarch.jpg "Architecture")

## Architecture Explained

- **IoHub** – To collect detected object and send it to cloud to further process using downstream system. Serves as the gateway to pump data out of the iot devices and send it to cloud. Also device management capability and secured data transmission are all provided by the Iot Hub.
- **Stream Analytics** – To read the data from incoming event hub in iot hub and parse the JSON and write back into Azure SQL and blob Storage. The incoming data set is in JSON format and that has to deflated to a structured format to be able to analysis. Also here we can do windowing based calculation if needed based on the use case requirements.
- **Azure SQL** – The detected object data with time at which object was detected are store for creating charts for web site and power bi based reporting. The data is only kept for 6 months to 1 year time frame for reporting and then cleanup job is created to delete the old records. Is used for Reporting and Dashboarding. Also used for downstream business systems if needed.
- **Blob Storage** – the same data going into Azure SQL is stored in blob for long term storage. Data older than one year or more is kept for auditing records and compliance. Preferably the images should also be stored here so that compliance and auditing can be performed when needed. Storing this data can also be moved to cold storage if needed to keep it for long term. More on long term and auditing purpose. Ability to analyze the model results based on historical data.
- **Web App** – Dashboard uses Azure SQL data to display the information in the page. It also uses historically data as well from Azure SQL but limited to the data stored in SQL.
- **Camera Tagging Module** - Here is another module to take pictures and tag them and send to custom vision service and also send to Blob Storage. Blob storage is used for long term and historical data analysis. In practical use case we wanted ability take real world pictures and then use them for training custom vision model and make the model more accurate.

To get started to add the module to vision kit follow the below link
https://github.com/balakreshnan/WorkplaceSafety/blob/master/CameraTaggingModule/readme.md

> For example in a real Factory or Plant or Hospital or any other scenario unless we have pictures it becomes hard to build model. With the above tagging module we can take the real world pictures and use that for training. The above module is based on manually taking pictures so that there is control on the picture taken and storage.

## Create a custom vision model using pictures available using custom vision service

- Log into customvision.ai and create a compact object detection project. 
- Upload the images with various tags. 
- Tag each image with the correct tag and bounding box. 
- Train the model and download the model as AI vision kit deploy files.
- For details please follow other articles in the vision ai documentation

## Deploy the model to Vision Kit

- Download the model file and upload to blob storage where we can access it for module twin. 
- You can zip the model and upload. 
- The ModelUrlZip property is assigned in the module twin to download the new vision model created. Now you should see the new model displayed the bounding box when you wear hard hat or vest or safety glass. The model should also predict Person as well.
- "https://bbiotstore.blob.core.windows.net/others/Model.zip"

## Create Report and Dashboard

> Create a resource group to process
> Create Storage blob

- Create a blob account
- Create a container
- Name the container as: visoinkitdata
- This is for long term storage of scored data to auditing and compliance and reporting purpose

> Create Azure SQL database

- Create a database
- Create a new table
- Here is the create table script:
- Create also the count table to aggregate how many objects detected every minute.
```
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
    CREATE TABLE [dbo].[visionkitinputs](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [confidence] [float] NULL,
        [label] [nvarchar](2000) NULL,
        [EventProcessedUtcTime] [datetime] NULL,
        [PartitionId] [int] NULL,
        [EventEnqueuedUtcTime] [datetime] NULL,
        [MessageId] [nvarchar](250) NULL,
        [CorrelationId] [nvarchar](250) NULL,
        [ConnectionDeviceId] [nvarchar](250) NULL,
        [ConnectionDeviceGenerationId] [nvarchar](2000) NULL,
        [EnqueuedTime] [datetime] NULL,
        [inserttime] [datetime] NULL
    ) ON [PRIMARY]
    GO
    ALTER TABLE [dbo].[visionkitinputs] ADD  CONSTRAINT [DF_visionkitinputs_inserttime]  DEFAULT (getdate()) FOR [inserttime]
    GO
    
    /****** Object:  Table [dbo].[visionkitcount]    Script Date: 9/29/2019 7:24:20 AM ******/
	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	CREATE TABLE [dbo].[visionkitcount](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[Avgconfidence] [float] NULL,
		[label] [nvarchar](2000) NULL,
		[EventProcessedUtcTime] [datetime] NULL,
		[PartitionId] [int] NULL,
		[EventEnqueuedUtcTime] [datetime] NULL,
		[MessageId] [nvarchar](250) NULL,
		[CorrelationId] [nvarchar](250) NULL,
		[ConnectionDeviceId] [nvarchar](250) NULL,
		[ConnectionDeviceGenerationId] [nvarchar](2000) NULL,
		[EnqueuedTime] [datetime] NULL,
		[count] [int] NULL,
		[inserttime] [datetime] NULL
	) ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[visionkitcount] ADD  CONSTRAINT [DF_visionkitcount_inserttime]  DEFAULT (getdate()) FOR [inserttime]
	GO
```

> Create Stream Analytics

- Create new Stream Analytics Cloud job
    - Create input from Iot Hub to read the data coming from the vision kit modules.
    - Create 2 outputs
        - One for Blob storage: outputblob for long term storage
            - Folder pattern: data/{date}
            - Date Format: YYYY/MM/DD
            - The above is for making sure larger data set can be created as well.
            - In Future when reading the data can be read parallely as well.
            - Save as CSV so that multiple system can be used to view the data if needed.
        - Other for Azure SQL database that was created above
            - Select table as: visionkitinputs
            - Connect with SQL user name and password.
            - Leave everything else as default.
            - Also count the number of items detected and sent to Iot hub. 
    - Now create the query to read data from Iot Hub and then parse the JSON structure into table format – row and column format and send it to both outputs.
        - Query Explained below
```
            WITH visiondata AS (
                SELECT
                confidence
                ,label
                ,EventProcessedUtcTime
                ,PartitionId
                ,EventEnqueuedUtcTime
                ,IoTHub.MessageId as MessageId
                ,IoTHub.CorrelationId as CorrelationId
                ,IoTHub.ConnectionDeviceId as ConnectionDeviceId
                ,IoTHub.ConnectionDeviceGenerationId as ConnectionDeviceGenerationId
                ,IoTHub.EnqueuedTime as EnqueuedTime
            FROM
                input
            )
            SELECT confidence,label,EventProcessedUtcTime,
            PartitionId,EventEnqueuedUtcTime,
            MessageId,CorrelationId,ConnectionDeviceId,
            ConnectionDeviceGenerationId,EnqueuedTime INTO outputblob FROM visiondata

            SELECT confidence,label,EventProcessedUtcTime,
            PartitionId,EventEnqueuedUtcTime,
            MessageId,CorrelationId,ConnectionDeviceId,
            ConnectionDeviceGenerationId,EnqueuedTime INTO sqloutput FROM visiondata

            SELECT ConnectionDeviceId,label, 
            avg(confidence) as Avgconfidence, 
            count(*) as count,
            MIN(CAST(EventEnqueuedUtcTime AS DATETIME)) as EventEnqueuedUtcTime,
            MIN(CAST(EventProcessedUtcTime AS DATETIME)) as EventProcessedUtcTime,
            MIN(CAST(EnqueuedTime AS DATETIME)) as EnqueuedTime
            INTO sqlaggr
            FROM visiondata
            GROUP BY TUMBLINGWINDOW(second,60),ConnectionDeviceId,label

```
    - First part is CTE with is common table expression
    - To send the output into multiple output a temporary CTE is created and then selected columns are used to push that into Azure SQL table and Blob storage.
    - The 2 Select query is one for Blob output and other for Azure SQL output.
    - I am also saving Device meta data information to further reporting.

> Create Web App to display information

- Create a ASP NET form application
- Change the connection string for SQL based on what you created above.
- Objects.aspx is the page that displays all the details of object detection
- The page refreshes every 20 seconds.
- The application uses asp charts to display details as well.
- The queries are embedded inside the code itself for logic
- On a high level the pages shows 
    - Current predicted object and confidence and Time
    - Lag time is the time until the latest record was processed in the cloud side.
        - Lag time show if the device is sending data currently.
        - Data is received only when the object is detected.
        - So long lag time are acceptable
![alt text](https://github.com/balakreshnan/WorkplaceSafety/blob/master/currentstatus.jpg "Current Status")
- Latest Detection Status
    - Shows the current object and confidence with time
    - It takes a 2 minute windows of latest data and analyze 
    - Finds if Vest, Hard Hat and Safety glass are detected.
    - Find if a person exists.
    - Then if calculates and see if there is a violation.
    - For example, if a person is available and not hearing vest, safety glass or hard hat then the violation        count is incremented.
    - The page also counts the vest, hard hat and safety glass.
    - The above logic is purely fictitious and might have to change based on use case
    - This is only valid for Realtime purpose as no object tracking is available.
    - If object tracking, then we can do real time proper person count with violation as well.
- Last One Day
    - The data is aggregated over hour for a 24 hours day from that point.
    - The data is aggregated and displayed as bar charts for visual display.
    - Both Object count per hours and confidence per hour average is also plotted as chart display.
- Latest 10 objects
    - This is just the latest 10 records to see the current history 
![alt text](https://github.com/balakreshnan/WorkplaceSafety/blob/master/historical.jpg "24 Hour Data")
- Errors
    - Any runtime error is captured and displayed for troubleshooting purpose.
    - Haven’t had a chance to integrate with application insights.

## Future Improvements

- Improve the custom vision AI model
- Add object Tracking
- Sending alerts to others Business and alerting systems
- Ability to view images in Cloud and send to custom vision by selecting images.
- Customize the use case based on different industry and use cases
