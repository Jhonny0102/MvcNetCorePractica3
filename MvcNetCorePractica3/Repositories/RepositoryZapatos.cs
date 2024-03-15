using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCorePractica3.Data;
using MvcNetCorePractica3.Models;
using System.Data;

#region Procedure

//create procedure SP_ZAPAS_OUT 
//(@posicion int, @idproducto int, @registros int out) 
//as 
//	select @registros = COUNT(IDPRODUCTO) from IMAGENESZAPASPRACTICA where IDPRODUCTO=@idproducto 
//	select IDIMAGEN, IDPRODUCTO, IMAGEN from  
//		(select cast( 
//		ROW_NUMBER() OVER (ORDER BY IDPRODUCTO) as int) AS POSICION 
//		, IDIMAGEN, IDPRODUCTO, IMAGEN
//		from IMAGENESZAPASPRACTICA 
//		where IDPRODUCTO = @idproducto) as QUERY 
//		where QUERY.POSICION >= @posicion and QUERY.POSICION < (@posicion + 1) 
//go 

//CREATE VIEW V_ZAPASPRACTICA
//AS
//	SELECT CAST( ROW_NUMBER() OVER (ORDER BY IDPRODUCTO) AS INT) AS POSICION,
//	ISNULL(IDPRODUCTO,0) AS IDPRODUCTO, NOMBRE ,DESCRIPCION , PRECIO
//	FROM ZAPASPRACTICA
//GO

//CREATE VIEW V_IMAGESZAPASPRACTICA
//AS
//	SELECT CAST(ROW_NUMBER() OVER (ORDER BY IMAGEN) AS INT) AS POSICION,
//	IDIMAGEN, IDPRODUCTO,IMAGEN
//	FROM IMAGENESZAPASPRACTICA
//GO

#endregion

namespace MvcNetCorePractica3.Repositories
{
    public class RepositoryZapatos
    {
        private ZapatosContext context;
        public RepositoryZapatos(ZapatosContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapato>> GetZapatosAsync()
        {
            return await this.context.Zapatos.ToListAsync();
        }

        public async Task<Zapato> FindZapatoAsync(int idProducto)
        {
            return await this.context.Zapatos.Where(z => z.IdProducto == idProducto).FirstOrDefaultAsync();
        }

        //Vamos a crear la paginacion de images zapas
        public async Task<ModelPaginacionZapas> GetZapatosImagenes(int posicion , int idproducto)
        {
            string sql = "SP_ZAPAS_OUT @posicion, @idproducto, @registros out";

            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);

            SqlParameter pamIdproducto = new SqlParameter("@idproducto", idproducto);

            SqlParameter pamRegistros = new SqlParameter("@registros", -1);

            pamRegistros.Direction = ParameterDirection.Output;

            var consulta = this.context.ImagenZapatos.FromSqlRaw(sql, pamPosicion, pamIdproducto, pamRegistros);

            List<ImagenZapatos> imagenes = await consulta.ToListAsync();

            int registros = (int)pamRegistros.Value;

            Zapato zapas = await this.FindZapatoAsync(idproducto);

            return new ModelPaginacionZapas

            {

                NumReigstro = registros,
                ImagenZapatos = imagenes,
                zapato = zapas

            };
        }

        public void InsertImagen(List<string> imagenes , int idproducto, int idimagen)
        {
            foreach (String img in imagenes)
            {
                ImagenZapatos imagenNuevo = new ImagenZapatos();
                imagenNuevo.IdImagen = idimagen;
                imagenNuevo.IdProducto = idproducto;
                imagenNuevo.Imagen = img;
                this.context.ImagenZapatos.Add(imagenNuevo);
                this.context.SaveChanges();
            }
        }

    }
}
