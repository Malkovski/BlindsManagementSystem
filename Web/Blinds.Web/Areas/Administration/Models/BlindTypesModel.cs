namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Web.Models;
    using Kendo.Mvc.UI;

    public class BlindTypesModel : MenuModel, IMapFrom<BlindType>, IDeletableEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public bool HasImage { get; set; }

        [Required(ErrorMessage = GlobalConstants.BlindTypeRequireText)]
        [MinLength(5, ErrorMessage = "Минимум 5 символа")]
        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        [UIHint("SingleLineTemplate")]
        public string Name { get; set; }

        [DisplayName(GlobalConstants.PriceDisplay)]
        [UIHint("SingleLineTemplate")]
        public string Price { get; set; }

        [Required(ErrorMessage = GlobalConstants.InfoRequireText)]
        [MinLength(20, ErrorMessage = "Информацията за продукта трябва да е минимум 20 символа!")]
        [DisplayName(GlobalConstants.InfoDisplay)]
        [UIHint("MultiLineTemplate")]
        public string Info { get; set; }

        public byte[] Content { get; set; }

        [DisplayName(GlobalConstants.ContentDisplay)]
        [UIHint("UploadTemplate")]
        public HttpPostedFileBase File { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<BlindTypesModel> Get()
        {
            return this.RepoFactory.Get<BlindTypeRepository>().GetActive()
                .Project()
                .To<BlindTypesModel>()
                .ToList();
        }

        public DataSourceResult Save(BlindTypesModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<BlindTypeRepository>();
                var entity = repo.GetById(viewModel.Id);

                var exists = repo.GetIfExists(viewModel.Name, viewModel.Id);

                if (exists)
                {
                    return new DataSourceResult
                    {
                        Errors = "Типът щора вече съществува!"
                    };
                }

                if (entity == null)
                {
                    entity = new BlindType();
                    repo.Add(entity);
                }

                Mapper.Map(viewModel, entity);

                if (viewModel.File != null)
                {
                    int fileSizeInBytes = viewModel.File.ContentLength;
                    MemoryStream target = new MemoryStream();
                    viewModel.File.InputStream.CopyTo(target);
                    entity.Content = target.ToArray();
                }

                repo.SaveChanges();
                viewModel.Id = entity.Id;
                viewModel.File = null;
                return null;
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }

        public DataSourceResult Delete(BlindTypesModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<BlindTypeRepository>();
                var entity = repo.GetById(viewModel.Id);

                entity.Deleted = true;
                entity.DeletedOn = DateTime.Now;

                repo.SaveChanges();
                return null;
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }
    }
}