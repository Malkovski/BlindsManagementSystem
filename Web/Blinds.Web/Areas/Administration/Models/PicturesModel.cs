namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using Blinds.Web.Models;
    using Blinds.Contracts;
    using Blinds.Web.Infrastructure.Mapping;
    using Blinds.Data.Models;
    using System.ComponentModel;
    using Common;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Data.Repositories;
    using System.Linq;
    using System.Web;
    using Kendo.Mvc.UI;
    using AutoMapper;
    using System.IO;
    using System.Data.Entity.Validation;
    using System.Text;
    public class PicturesModel : MenuModel, IModel<bool>, IMapFrom<Picture>, IMapTo<Picture>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [DisplayName(GlobalConstants.PictureOriginalFileNameDisplay)]
        [UIHint("SingleLineTemplate")]
        public string OriginalFileName { get; set; }

        [DisplayName(GlobalConstants.PictureOriginalSizeDisplay)]
        [UIHint("LongTemplate")]
        public int OriginalSize { get; set; }

        public byte[] Content { get; set; }

        public bool HasImage { get; set; }

        [DisplayName(GlobalConstants.ContentDisplay)]
        [UIHint("PictureUploadTemplate")]
        public HttpPostedFileBase [] Files { get; set; }

        [Required(ErrorMessage = GlobalConstants.BlindTypeRequireText)]
        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        [UIHint("DropDownTemplate")]
        public int BlindTypeId { get; set; }

        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        public string BlindTypeName { get; set; }

        public IEnumerable<SelectListItem> BlindTypes { get; set; }

        public void Init(bool init)
        {
            base.Init();

            this.BlindTypes = this.RepoFactory.Get<BlindTypeRepository>().GetAll()
                         .Select(c => new SelectListItem
                         {
                             Value = c.Id.ToString(),
                             Text = c.Name
                         }).ToList();
        }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<PicturesModel> Get()
        {
            return this.RepoFactory.Get<PictureRepository>()
                .All()
                .To<PicturesModel>()
                .ToList();
        }

        public IEnumerable<PicturesModel> GetByType(int id)
        {
            return this.RepoFactory.Get<PictureRepository>()
                .All()
                .Where(x => x.BlindTypeId == id)
                .To<PicturesModel>()
                .ToList();
        }

        public PicturesModel GetById(int id)
        {
            return this.RepoFactory.Get<PictureRepository>()
                .All()
                .Where(x => x.Id == id)
                .To<PicturesModel>()
                .FirstOrDefault();
        }

        public DataSourceResult Save(PicturesModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<PictureRepository>();

                MemoryStream target = null;

                if (viewModel.Files != null)
                {
                    foreach (var file in viewModel.Files)
                    {
                        int fileSizeInBytes = file.ContentLength;
                        target = new MemoryStream();
                        file.InputStream.CopyTo(target);

                        var entity = new Picture
                        {
                            BlindTypeId = viewModel.BlindTypeId,
                            OriginalFileName = file.FileName,
                            Extension = file.ContentType,
                            OriginalSize = file.ContentLength,
                            Content = target.ToArray()
                        };

                        repo.Add(entity);
                    }
                }

                try
                {
                    repo.SaveChanges();
                    viewModel.Id = 0;
                    viewModel.Files = null;
                    return null;
                }
                catch (DbEntityValidationException e)
                {
                    StringBuilder builder = new StringBuilder();

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        builder.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            builder.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                    }

                    return new DataSourceResult
                    {
                        Errors = builder
                    };
                }
            }
            else
            {
                return base.HandleErrors(modelState);
            }
        }

        public DataSourceResult Destroy(PicturesModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<PictureRepository>();
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

        // Mappings
        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Picture, PicturesModel>()
                .ForMember(s => s.BlindTypeName, opt => opt.MapFrom(u => u.BlindType.Name));
        }
    }
}