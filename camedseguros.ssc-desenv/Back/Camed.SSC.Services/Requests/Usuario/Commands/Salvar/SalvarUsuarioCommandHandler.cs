using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Core.Security;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using Camed.SSC.Core;

namespace Camed.SSC.Application.Requests
{
    public class SalvarUsuarioCommandHandler : HandlerBase, IRequestHandler<SalvarUsuarioCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IMediator mediator;

        public SalvarUsuarioCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            this.uow = uow;
            this.mediator = mediator;
        }

        public async Task<IResult> Handle(SalvarUsuarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.IsValid())
                {
                    base.result.SetValidationErros(request);
                    return await Task.FromResult(result);
                }

                var senha = "";

                if (uow.GetRepository<Usuario>().QueryAsync(u => u.Id != request.Id && u.Matricula == request.Matricula && u.Login == request.Login && u.Excluido == true && request.CPF != "00000000001").Result.Any())
                {
                    base.result.Message = "reativarExcluido";
                    return await Task.FromResult(result);
                }

                else if (request.CPF == "00000000001")
                {
                    var usuario = await uow.GetRepository<Domain.Entities.Usuario>().QueryFirstOrDefaultAsync(u => u.Id != request.Id && u.Matricula == request.Matricula && u.Login == request.Login && u.Excluido == true);
                    usuario.Excluido = false;

                    uow.GetRepository<Domain.Entities.Usuario>().Update(usuario);

                    await uow.CommitAsync();

                    return await Task.FromResult(result);
                }

                else if (uow.GetRepository<Usuario>().QueryAsync(u => u.Id != request.Id && u.Matricula == request.Matricula).Result.Any() && request.CPF != "00000000001")
                {
                    throw new ApplicationException("Matrícula já cadastrada");
                }

                //Solicitado: CPF pode ser repetido entre os usuários
                
                /*if (uow.GetRepository<Usuario>().QueryAsync(u => u.Id != request.Id && u.CPF == request.CPF).Result.Any())
                {
                    throw new ApplicationException("CPF já cadastrado");
                }*/

                else if (uow.GetRepository<Usuario>().QueryAsync(u => u.Id != request.Id && u.Login == request.Login).Result.Any() && request.CPF != "00000000001")
                {
                    throw new ApplicationException("Login já cadastrado");
                }

                    

                if (request.Id == 0)
                {
                    senha = new Random().Next(999999).ToString().PadLeft(6);



                    Domain.Entities.Usuario usuario = new Domain.Entities.Usuario
                    {
                        Nome = request.Nome,
                        Login = request.Login,
                        Senha = Encryption.CreateMD5Hash(senha),
                        Matricula = request.Matricula,
                        CPF = request.CPF.SomenteNumeros(),
                        Email = request.Email,
                        EnviarSLA = request.EnviarSLA,
                        EhCalculista = request.EhCalculista
                    };

                    usuario.Empresa = await uow.GetRepository<Empresa>().GetByIdAsync(request.Empresa.Value) ?? throw new ApplicationException("Empresa não foi localizada na base de dados");

                    if (request.Agencia.HasValue)
                    {
                        usuario.Agencia = await uow.GetRepository<Agencia>().GetByIdAsync(request.Agencia.Value) ?? throw new ApplicationException("Agência não foi localizada na base de dados");
                    }

                    usuario.GruposAgencias.UpdateCollection(request.GruposAgencias, "GrupoAgencia_Id");
                    usuario.Grupos.UpdateCollection(request.Grupos, "Grupo_Id");
                    usuario.AreasDeNegocio.UpdateCollection(request.AreasDeNegocio, "AreaDeNegocio_Id");

                    await uow.GetRepository<Domain.Entities.Usuario>().AddAsync(usuario);
                }
                else
                {
                    var usuario = await uow.GetRepository<Domain.Entities.Usuario>().QueryFirstOrDefaultAsync(u => u.Id == request.Id, new[] { "Grupos", "GruposAgencias", "AreasDeNegocio" });

                    usuario.Nome = request.Nome;
                    usuario.Login = request.Login;
                    usuario.Matricula = request.Matricula;
                    usuario.CPF = request.CPF.SomenteNumeros();
                    usuario.Email = request.Email;
                    usuario.EnviarSLA = request.EnviarSLA;
                    usuario.EhCalculista = request.EhCalculista;

                    usuario.Empresa = await uow.GetRepository<Empresa>().GetByIdAsync(request.Empresa.Value) ?? throw new ApplicationException("Empresa não foi localizada na base de dados");

                    if (request.Agencia.HasValue)
                    {
                        usuario.Agencia = await uow.GetRepository<Agencia>().GetByIdAsync(request.Agencia.Value) ?? throw new ApplicationException("Agência não foi localizada na base de dados");
                    }

                    usuario.GruposAgencias.UpdateCollection(request.GruposAgencias, "GrupoAgencia_Id");
                    usuario.Grupos.UpdateCollection(request.Grupos, "Grupo_Id");
                    usuario.AreasDeNegocio.UpdateCollection(request.AreasDeNegocio, "AreaDeNegocio_Id");

                    uow.GetRepository<Domain.Entities.Usuario>().Update(usuario);

                    
                }

                await uow.CommitAsync();

                if (!request.Agencia.HasValue)
                {
                    String sql = @"UPDATE USUARIO SET AGENCIA_ID = NULL WHERE ID =" + request.Id;

                    using (SqlConnection conn = new SqlConnection(ConnectionString.App))
                    {
                        conn.Open();

                        using (SqlCommand command = new SqlCommand(sql, conn)) { command.ExecuteNonQuery(); }
                    }
                }

                if (request.Id == 0)
                {
                    await mediator.Publish(new EmailNovoUsuarioNotification
                    {
                        Login = request.Login,
                        Senha = senha
                    });
                }

                return await Task.FromResult(result);
            }
            catch(Exception e)
            {
                var msg = "";

                if (!String.IsNullOrEmpty(e.InnerException?.Message))
                    msg = e.InnerException.Message;
                else
                    msg = e.Message;

                throw new ApplicationException("Erro: " + msg);
            }
            
        }
    }
}
