using System.Collections.Generic;
using System.Linq;

namespace DynamicForms.Models
{
    public sealed class UsuarioSingleton
    {
        static UsuarioSingleton _instance;
        public static UsuarioSingleton Instance
        {
            get { return _instance ?? (_instance = new UsuarioSingleton()); }
        }
        private UsuarioSingleton()
        {
            _usuarios = new List<T_Usuario>();
        }

        private List<T_Usuario> _usuarios;

        public T_Usuario ObterUsuario(int USE_ID)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.USE_ID == USE_ID);
            return usuario;
        }

        public void InserirUsuario(T_Usuario usuario)
        {
            int index = _usuarios.FindIndex(u => u.USE_ID == usuario.USE_ID);

            if (index == -1)
                _usuarios.Add(usuario);
            else
                _usuarios[index] = usuario;
        }

        public void RemoverUsuario(int USE_ID)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.USE_ID == USE_ID);

            if (usuario != null)
                _usuarios.Remove(usuario);
        }
    }
}
