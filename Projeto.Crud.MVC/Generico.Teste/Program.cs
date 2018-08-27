using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generico.Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Models.Repositorio<Models.Estatu> obj = new Models.Repositorio<Models.Estatu>())
            {
                obj.Exception += obj_exception;
                //obj.Create(new Models.Estatu() { Descripcion = "Idisponi"});
                //obj.Update(x => x.Id == 3, "Descripcion", "Pro-Alter");
                obj.Delete(obj.Retrieve(x => x.Id == 3));

                var listado = obj.Filter(x => true);
                foreach (var item in listado)
                {
                    Console.WriteLine(item.Descripcion);
                }
            }

            Console.WriteLine("Pressione <enter> para sair");
            Console.ReadLine();
        }

        private static void obj_exception(object sender, Models.ExceptionEventArgs e)
        {
            Console.WriteLine(string.Format("mensgaem de error: {0}", e.message));
            if(e.EntityValidationErros != null)
            {
                foreach(var item in e.EntityValidationErros)
                {
                    foreach (var error in item.ValidationErrors)
                    {
                        Console.WriteLine(string.Format("Mensagem: {0}, PropertyName{1}", error.ErrorMessage, error.PropertyName));
                    }
                }
            }

        }

    }
}
