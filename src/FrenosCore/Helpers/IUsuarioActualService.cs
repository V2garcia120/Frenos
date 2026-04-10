namespace FrenosCore.Helpers
{
    public interface IUsuarioActualService
    {
        int Id { get; }
        string Nombre { get; }
        string Rol { get; }

        string Ip { get; }


    }

}
