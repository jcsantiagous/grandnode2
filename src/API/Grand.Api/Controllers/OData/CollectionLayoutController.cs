﻿using Grand.Api.Queries.Models.Common;
using Grand.Business.Core.Interfaces.Common.Security;
using Grand.Business.Core.Utilities.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Grand.Api.Controllers.OData
{
    public class CollectionLayoutController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;

        public CollectionLayoutController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [SwaggerOperation(summary: "Get entity from CollectionLayout by key", OperationId = "GetCollectionLayoutById")]
        [HttpGet("{key}")]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Maintenance)) return Forbid();

            var layout = await _mediator.Send(new GetLayoutQuery { Id = key, LayoutName = typeof(Domain.Catalog.CollectionLayout).Name });
            if (!layout.Any()) return NotFound();

            return Ok(layout.FirstOrDefault());
        }

        [SwaggerOperation(summary: "Get entities from CollectionLayout", OperationId = "GetCollectionLayouts")]
        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Maintenance)) return Forbid();

            return Ok(await _mediator.Send(new GetLayoutQuery { LayoutName = typeof(Domain.Catalog.CollectionLayout).Name }));
        }
    }
}
