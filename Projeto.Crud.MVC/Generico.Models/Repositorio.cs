using MetodosDeExtension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Generico.Models
{

    public delegate void ExceptionEventHandle(object sender, ExceptionEventArgs e);


    public class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : class
    {
        public event ExceptionEventHandle Exception;

        CRUD_MVCEntities Context;

        public Repositorio()
        {
            Context = new CRUD_MVCEntities();
        }

        private DbSet<TEntity> EntitySet { get { return Context.Set<TEntity>(); }}


        public TEntity Create(TEntity toCreate)
        {
            TEntity Resultado = null;
            try
            {
                Resultado = EntitySet.Add(toCreate);
                Context.SaveChanges();
            }
            catch(DbEntityValidationException ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace, EntityValidationErros = ex.EntityValidationErrors});
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace});
                this.EscribirEnArchivoLog(ex);
            }

            return Resultado;
        }



        public bool Delete(TEntity toDelete)
        {
            bool Resultado = false;
            try
            {
                EntitySet.Attach(toDelete);
                EntitySet.Remove(toDelete);
                Resultado = Context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace, EntityValidationErros = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace });
                this.EscribirEnArchivoLog(ex);
            }

            return Resultado;
        }

        public List<TEntity> Filter(Expression<Func<TEntity, bool>> criterio)
        {
            List<TEntity> Resultado = new List<TEntity>();

            try
            {
                Resultado = EntitySet.Where(criterio).ToList();
            }
            catch (DbEntityValidationException ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace, EntityValidationErros = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace });
                this.EscribirEnArchivoLog(ex);
            }

            return Resultado;
        }

        public TEntity Retrieve(Expression<Func<TEntity, bool>> criterio)
        {
            TEntity Resultado = null;
            try
            {
                Resultado = EntitySet.FirstOrDefault(criterio);

            }
            catch (DbEntityValidationException ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace, EntityValidationErros = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace });
                this.EscribirEnArchivoLog(ex);
            }

            return Resultado;
        }


        public bool Update(TEntity toUpdate)
        {
            bool Resultado = false;
            try
            {
                EntitySet.Attach(toUpdate);
                Context.Entry<TEntity>(toUpdate).State = EntityState.Modified;
                Resultado = Context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace, EntityValidationErros = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace });
                this.EscribirEnArchivoLog(ex);
            }

            return Resultado;

        }



        public bool Update(Expression<Func<TEntity, bool>> criterio, string propertyName, object valor)
        {
            bool Resultado = false;
            try
            {
                Context.Entry<TEntity>(EntitySet.FirstOrDefault(criterio)).Property(propertyName).CurrentValue = valor;
                Resultado = Context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace, EntityValidationErros = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Exception?.Invoke(this, new ExceptionEventArgs() { InnerException = ex.InnerException, message = ex.Message, Source = ex.Source, StrackTrace = ex.StackTrace });
                this.EscribirEnArchivoLog(ex);
            }

            return Resultado;

        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }

    }


    public class ExceptionEventArgs:EventArgs
    {
        public string message { get; set; }
        public string Source { get; set; }
        public string StrackTrace { get; set; }
        public Exception InnerException { get; set; }
        public MethodBase TargetSite { get; set; }
        public IEnumerable<DbEntityValidationResult> EntityValidationErros { get; set; }
    }
}
