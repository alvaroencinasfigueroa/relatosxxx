namespace Relatosxxx.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Metodo { get; set; } = string.Empty; // "TON" o "USDT"
        public string Identificador { get; set; } = string.Empty; // Memo o TxID
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public bool Procesado { get; set; } = false;

        // Navegación
        public Usuario Usuario { get; set; } = null!;
    }
}