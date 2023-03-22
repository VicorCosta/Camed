create or alter procedure [dbo].[usp_AuditLog] 
(
	@datainicial datetime, 
	@datafinal datetime, 
	@usuario varchar(max) = null,
	@evento int = null, 
	@tabela varchar(100) = null, 
	@chave int = null, 
	@numerosolicitacao int = null, 
	@mensagem varchar(max) = null
)
as   
begin
	select
		cast('TAB' as nvarchar(3)) as TpReg,      
		AuditLogId as Id,      
		cast(AuditLogId as varchar(32)) as OriginalId,      
		UserName,      
		cast(SWITCHOFFSET(EventDateUTC,'-03:00') as datetime) as EventTime,      
		cast(case EventType       
		 when '0' then 'Insert'      
		 when '1' then 'Delete'      
		 when '2' then 'Update'      
		end as nvarchar(256)) as EventType,      
		TableName,      
		cast(RecordId as int) as Chave,      
		Numero as NumeroDaSolicitacao,      
		cast(null as nvarchar(1024)) as Message,      
		cast(null as nvarchar(256)) as MachineName,      
		cast(null as nvarchar(1024)) as RequestUrl,      
		cast(null as nvarchar(2048)) as Details 

	from AuditLog 
	left outer join Solicitacao on AuditLog.RecordId = Solicitacao.Numero

	where cast(SWITCHOFFSET(EventDateUTC,'-03:00') as datetime) between @datainicial and @datafinal
	and (@usuario is null or UserName like '%'+@usuario+'%')
	and (@evento is null or EventType = cast(@evento as int))
	and (@chave is null or cast(RecordId as int) =  @chave) 
	and (@numerosolicitacao is null or Numero = @numerosolicitacao)
	and (@tabela is null or AuditLog.TableName = @tabela)


	union all    
    
	select
		'LOG' as TpReg,     
		CONVERT(int, CONVERT(VARBINARY, EventId, 2)) as Id,   
		cast(EventId as varchar(32)) as OriginalId,  
		dbo.fn_ObterUsuarioDoAuditEvent(details) as UserName,      
		EventTime,      
		case substring(EventType, len(EventType) - 16, 17) 
			when 'FailureAuditEvent' then 'Falha na Autenticação'
			when 'SuccessAuditEvent' then 'Sucesso na Autenticação'
			else EventType
		end as EventType,      
		null as TableName,      
		null as RecordId,      
		null as NumeroDaSolicitacao,      
		Message,      
		MachineName,      
		RequestUrl,      
		cast(Details as nvarchar(2048)) as Details   
		
	from aspnet_WebEvent_Events      
	where EventTime between @datainicial and @datafinal
	and (@mensagem is null or [Message] like '%' + @mensagem + '%')
end

