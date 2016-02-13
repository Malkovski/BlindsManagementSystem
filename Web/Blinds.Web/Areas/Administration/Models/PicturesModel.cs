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
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    using AutoMapper;
    using System.IO;

    public class PicturesModel : MenuModel, IModel<bool>, IMapFrom<Picture>, IHaveCustomMappings, IDeletableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.PictureTitleRequireText)]
        [MinLength(5, ErrorMessage = GlobalConstants.NameMinLength)]
        [DisplayName(GlobalConstants.PictureTitleDisplay)]
        [UIHint("SingleLineTemplate")]
        public string Title { get; set; }

        [DisplayName(GlobalConstants.PictureOriginalFileNameDisplay)]
        [UIHint("SingleLineTemplate")]
        public string OriginalFileName { get; set; }

        [DisplayName(GlobalConstants.PictureOriginalSizeDisplay)]
        [UIHint("LongTemplate")]
        public int OriginalSize { get; set; }

        [DisplayName(GlobalConstants.PictureExtensionDisplay)]
        [UIHint("SingleLineTemplate")]
        public string Extension { get; set; }

        public byte[] Content { get; set; }

        public bool HasImage { get; set; }

        [DisplayName(GlobalConstants.ContentDisplay)]
        [UIHint("PictureUploadTemplate")]
        public HttpPostedFileBase File { get; set; }

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
                .Project()
                .To<PicturesModel>()
                .ToList();
        }

        public IEnumerable<PicturesModel> GetByType(int id)
        {
            return this.RepoFactory.Get<PictureRepository>()
                .All()
                .Where(x => x.BlindTypeId == id)
                .Project()
                .To<PicturesModel>()
                .ToList();
        }

        public PicturesModel GetById(int id)
        {
            return this.RepoFactory.Get<PictureRepository>()
                .All()
                .Where(x => x.Id == id)
                .Project()
                .To<PicturesModel>()
                .FirstOrDefault();
        }

        public DataSourceResult Save(PicturesModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel != null && modelState.IsValid)
            {
                var repo = this.RepoFactory.Get<PictureRepository>();
                var entity = repo.GetById(viewModel.Id);

                var exists = repo.GetIfExists(viewModel.Title, viewModel.Id);

                if (exists)
                {
                    return new DataSourceResult
                    {
                        Errors = "Picture with this name already exists!"
                    };
                }

                if (entity == null)
                {
                    entity = new Picture();
                    repo.Add(entity);
                }

                Mapper.Map(viewModel, entity);

                if (viewModel.File != null)
                {
                    entity.OriginalFileName = viewModel.File.FileName;
                    entity.Extension = viewModel.File.ContentType;
                    entity.OriginalSize = viewModel.File.ContentLength;

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
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Picture, PicturesModel>();

            configuration.CreateMap<PicturesModel, Picture>().ReverseMap()
                .ForMember(s => s.BlindTypeName, opt => opt.MapFrom(u => u.BlindType.Name));
        }
    }
}