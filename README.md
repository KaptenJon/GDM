**Generic Data Management (GDM) Tool** is a tool for scheduled data processing. It is currently used to process data formatted in tables into data in other tables.

**The tool process:**
Data Harvesting from SLQ, Excel or CSV files->Do operations i.e. Filter, sort, process -> Data Export into Excel, *Text, SQL or specialized formats.
![](Home_gdm.png)

**Developers**
The project is Plugin based. Every operation, input or output can be added by developers. The you need to reference the interface project and create one class that extend ITool, IInput or IOutput as well as create one class that extends the IPluginSettings. The output dll can then be placed in the pluginfolder to be dynamically loaded into GDM.

**Project Funding**
This project is partly created in a research project called Streamod funded by Vinnova.

**This Release**
* 21/7
	* Fixed Merge
	* CMSD output has been significantly extended. Videos about how to use it will hopefully come.
* 19/7
	* CMSD output fixed
* 16/6 
	* File batch bug fixed
* 26/5
	* Added a export to SQL plugin, only tested with MS-SQL Server
* 24/5
	* Now the status bar in left corner should work
	* It is now possible to filter date columns relative. eg the 7 days minus current date. This also works for custom plugins. [[CurrentDateTime]([CurrentDateTime)] translates to current date [[CurrentDateTime-dddd:hh:mm:ss:fff]([CurrentDateTime-dddd_hh_mm_ss_fff)] is the format for relative time
* 12/5
	* Added new plugin to sum or create averages of sequences of operation. You get a sum for each individual sequence recorded in the table.
* 4/5
	* Fixed bug when no distribution fit the data in the plugin Generate Distributions
* 3/5
	* You can now choose among the distributions.


## Short introduction video  [https://youtu.be/36qLHbHc--k](https://youtu.be/36qLHbHc--k) 
{video:url=http://www.youtube.com/watch?v=36qLHbHc--k,type=youtube}