/*

CREATE TABLE [dbo].[JobLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL
)

CREATE PROCEDURE [dbo].[MyJobFunction]
AS BEGIN
	INSERT INTO [dbo].[JobLog]
           ([Message])
     VALUES  (CAST(CURRENT_TIMESTAMP AS nvarchar(20)))

END


CREATE PROCEDURE [dbo].[DropJobByNameIfExists] @jobName nvarchar(40)
AS
BEGIN
	DECLARE @jobId binary(16);
	DECLARE @foundCount int = (SELECT COUNT(job_id) FROM msdb.dbo.sysjobs WHERE (name = @jobName));
	WHILE @foundCount > 0 BEGIN
		SET @foundCount = @foundCount - 1;
		SELECT @jobId = job_id FROM msdb.dbo.sysjobs WHERE (name = @jobName)
		IF (@jobId IS NOT NULL)
		BEGIN
			EXEC msdb.dbo.sp_delete_job @jobId
		END
	END
END

ALTER PROCEDURE [dbo].[DropScheduleByNameIfExists] @scheduleName nvarchar(40)
AS
BEGIN
	DECLARE @scheduleId int;
	DECLARE @foundCount int = (SELECT COUNT(schedule_id) FROM msdb.dbo.sysschedules WHERE (name = @scheduleName));
	WHILE @foundCount > 0
	BEGIN
		SET @foundCount = @foundCount - 1;
		SELECT @scheduleId = schedule_id FROM msdb.dbo.sysschedules WHERE (name = @scheduleName)
		IF (@scheduleId IS NOT NULL)
		BEGIN
			EXEC msdb.dbo.sp_delete_schedule @scheduleId
		END
	END
END*/



USE msdb;
GO

EXEC [dbo].[DropJobByNameIfExists] @jobName = N'RT_InitPeriod'
EXEC [dbo].[DropScheduleByNameIfExists] @scheduleName = N'RT_RunOnce'
EXEC [dbo].[DropScheduleByNameIfExists] @scheduleName = N'RT_RunOnFirstDayOfTheMonth'
EXEC [dbo].[DropScheduleByNameIfExists] @scheduleName = N'RT_RunEveryDay'
EXEC [dbo].[DropScheduleByNameIfExists] @scheduleName = N'RT_RunEveryHour'
EXEC [dbo].[DropScheduleByNameIfExists] @scheduleName = N'RT_RunEveryMinute'


EXEC dbo.sp_add_job
    @job_name = N'RT_InitPeriod',
	@enabled = 1,
	@description = N'Init monthly period';
GO
EXEC sp_add_jobstep
    @job_name = N'RT_InitPeriod',
    @step_name = N'RunFunction',
    @subsystem = N'TSQL',
    @command = N'EXEC MyApp.[dbo].MyJobFunction', 
    @retry_attempts = 5,
    @retry_interval = 5 ;
GO
EXEC dbo.sp_add_schedule
    @schedule_name = N'RT_RunOnce',
    @freq_type = 1,
	@freq_recurrence_factor = 0,
    @active_start_time = 000000,
	@active_end_time   = 235959;
GO
EXEC dbo.sp_add_schedule
    @schedule_name = N'RT_RunOnFirstDayOfTheMonth',
    @freq_type = 16,
	@freq_interval = 1,			 --1st day of month
	@freq_recurrence_factor = 1, --every 1st month
    @active_start_time = 000000,
	@active_end_time   = 235959;
GO
EXEC dbo.sp_add_schedule
    @schedule_name = N'RT_RunEveryDay',
    @freq_type = 4,
	@freq_interval = 1,
	@freq_recurrence_factor = 0,
	@active_start_time = 000000,
	@active_end_time   = 235959;
GO
EXEC dbo.sp_add_schedule
    @schedule_name = N'RT_RunEveryHour',
    @freq_type = 4,
	@freq_interval = 1,
	@freq_recurrence_factor = 0,
	@freq_subday_type = 8, 
	@freq_subday_interval = 1,
	@active_start_time = 000000,
	@active_end_time   = 235959;
GO

EXEC dbo.sp_add_schedule
    @schedule_name = N'RT_RunEveryMinute',
    @freq_type = 4,
	@freq_interval = 1,
	@freq_recurrence_factor = 0,
	@freq_subday_type = 4, 
	@freq_subday_interval = 1,
	@active_start_time = 000000,
	@active_end_time   = 235959;
GO

EXEC sp_attach_schedule
   @job_name = N'RT_InitPeriod',
   @schedule_name = N'RT_RunOnFirstDayOfTheMonth';-- N'CollectorSchedule_Every_5min';
GO

EXEC dbo.sp_add_jobserver
    @job_name = N'RT_InitPeriod';
GO










