﻿namespace Blinds.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Blinds.Web.Areas.Administration.Models.Base;
    using Common;
    using Contracts;
    using Data.Models;
    using Data.Repositories;
    using Infrastructure.Mapping;
    using Web.Models;

    public class BlindTypesModel : AdminBaseModel, IMapFrom<BlindType>, IModel, IDeletableEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.BlindTypeRequireText)]
        [MinLength(5)]
        [DisplayName(GlobalConstants.BlindTypeDisplay)]
        [UIHint("SingleLineTemplate")]
        public string Name { get; set; }

        [Required(ErrorMessage = GlobalConstants.PriceRequireText)]
        [DisplayName(GlobalConstants.PriceDisplay)]
        [UIHint("SingleLineTemplate")]
        public string Price { get; set; }

        [Required(ErrorMessage = GlobalConstants.InfoRequireText)]
        [MinLength(100)]
        [DisplayName(GlobalConstants.InfoDisplay)]
        [UIHint("MultiLineTemplate")]
        public string Info { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void Init()
        {
        }

        public IEnumerable<BlindTypesModel> Get()
        {
            return this.RepoFactory.Get<BlindTypeRepository>().GetActive()
                .Project()
                .To<BlindTypesModel>()
                .ToList();
        }

        public void Save(BlindTypesModel viewModel)
        {
            var repo = this.RepoFactory.Get<BlindTypeRepository>();
            var entity = repo.GetById(viewModel.Id);

            if (entity == null)
            {
                entity = new BlindType();
                repo.Add(entity);
            }

            Mapper.Map(viewModel, entity);
            repo.SaveChanges();
            viewModel.Id = entity.Id;
        }

        public void Delete(BlindTypesModel viewModel)
        {
            var repo = this.RepoFactory.Get<BlindTypeRepository>();
            var entity = repo.GetById(viewModel.Id);

            entity.Deleted = true;
            entity.DeletedOn = DateTime.Now;

            repo.SaveChanges();
        }
    }
}