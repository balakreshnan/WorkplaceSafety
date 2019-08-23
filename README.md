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
IoHub – To collect detected object and send it to cloud to further process using downstream system. Serves as the gateway to pump data out of the iot devices and send it to cloud. Also device management capability and secured data transmission are all provided by the Iot Hub.
Stream Analytics – To read the data from incoming event hub in iot hub and parse the JSON and write back into Azure SQL and blob Storage. The incoming data set is in JSON format and that has to deflated to a structured format to be able to analysis. Also here we can do windowing based calculation if needed based on the use case requirements.
Azure SQL – The detected object data with time at which object was detected are store for creating charts for web site and power bi based reporting. The data is only kept for 6 months to 1 year time frame for reporting and then cleanup job is created to delete the old records. Is used for Reporting and Dashboarding. Also used for downstream business systems if needed.
Blob Storage – the same data going into Azure SQL is stored in blob for long term storage. Data older than one year or more is kept for auditing records and compliance. Preferably the images should also be stored here so that compliance and auditing can be performed when needed. Storing this data can also be moved to cold storage if needed to keep it for long term. More on long term and auditing purpose. Ability to analyze the model results based on historical data.
Web App – Dashboard uses Azure SQL data to display the information in the page. It also uses historically data as well from Azure SQL but limited to the data stored in SQL.

## Create a custom vision model using pictures available using custom vision service
Log into customvision.ai and create a compact object detection project. 
Upload the images with various tags. 
Tag each image with the correct tag and bounding box. 
Train the model and download the model as AI vision kit deploy files.
For details please follow other articles in the vision ai documentation

## Deploy the model to Vision Kit
Download the model file and upload to blob storage where we can access it for module twin. 
You can zip the model and upload. 
The ModelUrlZip property is assigned in the module twin to download the new vision model created. Now you should see the new model displayed the bounding box when you wear hard hat or vest or safety glass. The model should also predict Person as well.
"https://bbiotstore.blob.core.windows.net/others/Model.zip"

## Create Report and Dashboard
> Create a resource group to process



## Future Improvements
Improve the custom vision AI model
Add object Tracking
Sending alerts to others Business and alerting systems
Ability to view images in Cloud and send to custom vision by selecting images.
Customize the use case based on different industry and use cases

