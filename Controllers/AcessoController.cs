using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Controllers
{
    public class AcessoController : BaseController
    {
        [AllowAnonymous]
        public ActionResult SemAcesso()
        {
            return View();
        }

        // GET
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            //verifica se o usuário já está logado
            
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "DynamicWeb");

            return View();
        }


        // GET
        [AllowAnonymous]
        public ActionResult LoginColetor(string ReturnUrl)
        {
            //verifica se o usuário já está logado

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Coletor");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        [AllowAnonymous]
        public ActionResult Login(T_Usuario usuario, string ReturnUrl)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var valido = false;
                #region AuthCookie
                if (valido == false)
                {
                    if (string.IsNullOrEmpty(usuario.USE_EMAIL) || string.IsNullOrEmpty(usuario.USE_SENHA))
                        ViewBag.alerta = "Usuário ou Senha não informados!";
                    else
                    {
                        usuario.USE_SENHA = UtilPlay.GetSha1(usuario.USE_SENHA);
                        usuario = ConsultarUsuario(usuario, db);

                        if (usuario == null)
                        {
                            ViewBag.alerta = "Usuário ou Senha Inválidos!";
                        }
                        //else if (usuario.USE_ATIVO == 0)
                        //{
                        //    ViewBag.alerta = "Este usuário está desativado!";
                        //}
                        else
                        {

                            if (usuario.TURM_ID == null)
                            {
                                usuario.TURM_ID = "";
                            }
                            List<Claim> claims = new List<Claim>
                            {
                                new Claim("UserId", usuario.USE_ID.ToString()),
                                new Claim("UserName", usuario.USE_NOME),
                                new Claim("UserTurma", usuario.TURM_ID)
                            };

                            ClaimsIdentity identidadeDeUsuario = new ClaimsIdentity(claims, "Usuario");
                            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identidadeDeUsuario);

                            var propriedadesDeAutenticacao = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTime.UtcNow.ToLocalTime().AddDays(1), // Tempo de sessao do usuario logado
                                IsPersistent = true
                            };

                            HttpContext.SignInAsync
                                    (CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal,
                                    propriedadesDeAutenticacao);

                            if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/") && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                                return Redirect(ReturnUrl);

                            return RedirectToAction("Index", "DynamicWeb");

                        }
                    }
                }
                #endregion
                #region Auth AD
                //Acesso pelo AD
                else
                {
                    if (db.T_Usuario.Count(x => x.USE_EMAIL == usuario.USE_EMAIL) <= 0)
                    {
                        ViewBag.alerta = "Usuário encontrado no Activity Directory, porém o mesmo não possui perfil associado na ferramenta de indicadores";
                        return View(usuario);
                    }

                    T_Usuario user = db.T_Usuario
                                    .Where(u => u.USE_EMAIL == usuario.USE_EMAIL &&
                                        u.USE_SENHA == usuario.USE_SENHA)
                                    .Include(u => u.T_Usuario_Perfil)
                                    .ThenInclude(up => up.Perfil)
                                    .ThenInclude(p => p.T_PERFIL_OBJETO_CONTROLAVEL)
                                    .FirstOrDefault();

                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, user.USE_NOME));
                    claims.Add(new Claim(ClaimTypes.Email, user.USE_EMAIL));

                    foreach (var usuario_perfil in user.T_Usuario_Perfil)
                    {
                        var perfil = usuario_perfil.Perfil;
                        foreach (var obj_controlavel in perfil.T_PERFIL_OBJETO_CONTROLAVEL)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, obj_controlavel.OBJ_ID));
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    //Loga de fato
                    var result = HttpContext.SignInAsync(
                          CookieAuthenticationDefaults.AuthenticationScheme,
                          new ClaimsPrincipal(claimsIdentity),
                          authProperties);

                    if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/") &&
                        !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                        return Redirect(ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                #endregion
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [AllowAnonymous]
        public ActionResult LoginColetor(T_Usuario usuario, string ReturnUrl)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var valido = false;
                #region AuthCookie
                if (valido == false)
                {
                    if (string.IsNullOrEmpty(usuario.USE_EMAIL) || string.IsNullOrEmpty(usuario.USE_SENHA))
                        ViewBag.alerta = "Usuário ou Senha não informados!";
                    else
                    {
                        usuario.USE_SENHA = UtilPlay.GetSha1(usuario.USE_SENHA);
                        usuario = ConsultarUsuario(usuario, db);

                        if (usuario == null)
                        {
                            ViewBag.alerta = "Usuário ou Senha Inválidos!";
                        }
                        //else if (usuario.USE_ATIVO == 0)
                        //{
                        //    ViewBag.alerta = "Este usuário está desativado!";
                        //}
                        else
                        {

                            if (usuario.TURM_ID == null)
                            {
                                usuario.TURM_ID = "";
                            }
                            List<Claim> claims = new List<Claim>
                            {
                                new Claim("UserId", usuario.USE_ID.ToString()),
                                new Claim("UserName", usuario.USE_NOME),
                                new Claim("UserTurma", usuario.TURM_ID)
                            };

                            ClaimsIdentity identidadeDeUsuario = new ClaimsIdentity(claims, "Usuario");
                            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identidadeDeUsuario);

                            var propriedadesDeAutenticacao = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTime.UtcNow.ToLocalTime().AddDays(1), // Tempo de sessao do usuario logado
                                IsPersistent = true
                            };

                            HttpContext.SignInAsync
                                    (CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal,
                                    propriedadesDeAutenticacao);

                            if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/") && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                                return Redirect(ReturnUrl);

                            return RedirectToAction("Index", "Coletor");

                        }
                    }
                }
                #endregion
                #region Auth AD
                //Acesso pelo AD
                else
                {
                    if (db.T_Usuario.Count(x => x.USE_EMAIL == usuario.USE_EMAIL) <= 0)
                    {
                        ViewBag.alerta = "Usuário encontrado no Activity Directory, porém o mesmo não possui perfil associado na ferramenta de indicadores";
                        return View(usuario);
                    }

                    T_Usuario user = db.T_Usuario
                                    .Where(u => u.USE_EMAIL == usuario.USE_EMAIL &&
                                        u.USE_SENHA == usuario.USE_SENHA)
                                    .Include(u => u.T_Usuario_Perfil)
                                    .ThenInclude(up => up.Perfil)
                                    .ThenInclude(p => p.T_PERFIL_OBJETO_CONTROLAVEL)
                                    .FirstOrDefault();

                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, user.USE_NOME));
                    claims.Add(new Claim(ClaimTypes.Email, user.USE_EMAIL));

                    foreach (var usuario_perfil in user.T_Usuario_Perfil)
                    {
                        var perfil = usuario_perfil.Perfil;
                        foreach (var obj_controlavel in perfil.T_PERFIL_OBJETO_CONTROLAVEL)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, obj_controlavel.OBJ_ID));
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    //Loga de fato
                    var result = HttpContext.SignInAsync(
                          CookieAuthenticationDefaults.AuthenticationScheme,
                          new ClaimsPrincipal(claimsIdentity),
                          authProperties);

                    if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/") &&
                        !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                        return Redirect(ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                #endregion
            }
            return View();
        }

        //Metódo Validar Usuário
        public T_Usuario ConsultarUsuario(T_Usuario usuario, JSgi db)
        {
            T_Usuario user = db.T_Usuario.AsNoTracking()
                .Where(u => u.USE_EMAIL == usuario.USE_EMAIL && u.USE_SENHA == usuario.USE_SENHA)
                .Select(u => new T_Usuario
                {
                    USE_ID = u.USE_ID,
                    USE_NOME = u.USE_NOME,
                    TURM_ID = u.TURM_ID,
                    USE_ATIVO = u.USE_ATIVO
                })
                .FirstOrDefault();

            return user;
        }

        [AllowAnonymous]
        public ActionResult Logoff()
        {
            var usuarioLogdado = ObterUsuarioLogado();
            UsuarioSingleton.Instance.RemoverUsuario(usuarioLogdado.USE_ID);
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Acesso");
        }

        [AllowAnonymous]
        public ActionResult LogoffColetor()
        {
            var usuarioLogdado = ObterUsuarioLogado();
            UsuarioSingleton.Instance.RemoverUsuario(usuarioLogdado.USE_ID);
            HttpContext.SignOutAsync();
            return RedirectToAction("LoginColetor", "Acesso");
        }

        [Authorize]
        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Forgot(T_Usuario usuario)
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Perfil()
        {
            var user = new T_Usuario();
            int id = Convert.ToInt32(HttpContext.User.Identity.Name);
            using (var context = new ContextFactory().CreateDbContext(new string[] { }))
            {
                user = context.T_Usuario.Find(id);
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize]
        public ActionResult Perfil(T_Usuario usuario)
        {
            var u = HttpContext.User.Identity.Name;
            ModelState.Remove("SENHA");
            if (ModelState.IsValid)
            {
                using (var context = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    var user = context.T_Usuario.Find(usuario.USE_ID);
                    user.USE_NOME = usuario.USE_NOME;
                    if (usuario.USE_SENHA != "" && usuario.USE_SENHA != null)
                        usuario.USE_SENHA = UtilPlay.GetSha1(usuario.USE_SENHA);
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        #region ActiveDirectory
        /// <summary>
        /// Autentica acesso pelo AD
        /// </summary>
        /// <param name="IpServer">Endereço de ip do servidor</param>
        /// <param name="User">Usuário de acesso</param>
        /// <param name="Senha">Senha de acesso</param>
        /// <returns>Retorna true se autenticado e false se não foi autenticado</returns>
        //public bool Autentica(string IpServer, string User, string Senha)
        //{
        //    bool valido = false;
        //    try
        //    {
        //        DirectoryEntry objAD = new DirectoryEntry("LDAP://" + IpServer, User, Senha);
        //        var grupos = new List<string>();
        //        var grupoSgi = new List<T_Grupo>();
        //        var perfisAcesso = new List<T_Perfil>();
        //        if (objAD.Name != "")
        //            valido = true;
        //        //Valida se autentiou usuário no AD
        //        if (valido)
        //        {
        //            grupos = BuscaListadeGrupo(objAD);
        //            grupoSgi = db.T_Grupo.Where(x => grupos.Any(j => j.ToUpper() == x.NOME.ToUpper())).ToList();
        //            perfisAcesso = db.T_Perfil.Where(x => grupos.Any(j => j.ToUpper().Replace("PSGI_", "") == x.PER_NOME.ToUpper())).ToList();

        //            //Valida se usuário esta cadastrado no SGI
        //            if (db.T_Usuario.Count(x => x.USE_EMAIL == User) > 0)
        //            {
        //                var usuario = db.T_Usuario.First(x => x.USE_EMAIL == User);
        //                if (grupos.Count <= 0)
        //                    usuario.USE_ATIVO = (int)Enums.Ativo.Bloqueada;
        //                else
        //                {
        //                    var gruposUsuario = db.T_USER_GRUPO.Where(x => x.USE_ID == usuario.USE_ID).ToList();
        //                    db.T_USER_GRUPO.RemoveRange(gruposUsuario);
        //                    foreach (var item in grupoSgi)
        //                    {
        //                        if (usuario.T_USER_GRUPO.Count(x => x.GRU_ID == item.GRU_ID) <= 0)
        //                        {
        //                            usuario.T_USER_GRUPO.Add(new T_USER_GRUPO() { GRU_ID = item.GRU_ID, USE_ID = usuario.USE_ID });
        //                        }
        //                    }
        //                }
        //                if (perfisAcesso.Count > 0)
        //                {    //usuario.ID_PERFIL = perfisAcesso.FirstOrDefault().PER_ID;
        //                    usuario.T_Perfil = perfisAcesso;
        //                }

        //                db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
        //                db.SaveChanges();//Salva usuário
        //            }
        //            else if (grupoSgi.Count > 0)//Cadastra usuário
        //            {
        //                var usuario = new T_Usuario();
        //                if (perfisAcesso.Count > 0)
        //                {    //usuario.ID_PERFIL = perfisAcesso.FirstOrDefault().PER_ID;
        //                    usuario.T_Perfil = perfisAcesso;
        //                }
        //                else
        //                {
        //                    //usuario.ID_PERFIL = db.T_Perfil.FirstOrDefault(x => x.PER_NOME == "Padrão").PER_ID;
        //                    usuario.T_Perfil = db.T_Perfil.Where(x => x.PER_NOME == "Padrão").ToList();
        //                }

        //                usuario.USE_EMAIL = User;
        //                usuario.USE_NOME = User;
        //                usuario.USE_SENHA = FormsAuthentication.HashPasswordForStoringInConfigFile(Senha, "SHA1");
        //                foreach (var item in grupoSgi)
        //                {
        //                    if (usuario.T_USER_GRUPO.Count(x => x.GRU_ID == item.GRU_ID) <= 0)
        //                    {
        //                        usuario.T_USER_GRUPO.Add(new T_USER_GRUPO() { GRU_ID = item.GRU_ID, USE_ID = usuario.USE_ID });
        //                    }
        //                }
        //                db.T_Usuario.Add(usuario);
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //        throw new Exception(ex.Message);
        //    }

        //    return valido;
        //}

        /// <summary>
        /// Busca grupos de usuários no AD
        /// </summary>
        /// <param name="de">Objeto com o usuário autenticado.</param>
        /// <returns>Retorna lista de grupos</returns>
        //public List<string> BuscaListadeGrupo(DirectoryEntry de)
        //{

        //    var objSearchADAM = new DirectorySearcher(de);
        //    objSearchADAM.Filter = "(SAMAccountName=" + de.Username + ")";
        //    objSearchADAM.SearchScope = SearchScope.Subtree;
        //    var objSearchResults = objSearchADAM.FindOne();
        //    List<string> grupos = new List<string>();
        //    foreach (object oMember in objSearchResults.Properties["memberOf"])
        //    {
        //        var grupo = oMember.ToString().Split(',')[0].Replace("CN=", "");
        //        var filter = string.Format("(&(objectClass=group)(name={0}))", grupo);
        //        var ds = new DirectorySearcher(de, filter);
        //        var result = ds.FindOne();
        //        grupos.Add(grupo.Replace("CSGI_", ""));
        //        //Busca Sub Grupo);
        //        foreach (var subGrupo in result.Properties["memberOf"])
        //        {
        //            grupos.Add(subGrupo.ToString().Split(',')[0].Replace("CN=", "").Replace("CSGI_", ""));
        //        }
        //    }
        //    return grupos;
        //}

        #endregion


    }
}