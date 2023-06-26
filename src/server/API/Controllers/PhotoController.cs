﻿using API.ViewModels;
using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Models.DataTransferObjects;
using Models.Models;
using Models.SubModels;
using Models.SubModels.Account;
using System.Web.Http.Description;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        readonly IAccountRepository _accountRepository;
        readonly IGenericQuery _query;
        readonly IMapper _mapper;
        readonly AuthenticationController _authenticationController;

        public PhotoController(IAccountRepository account, IMapper mapper, AuthenticationController authenticationController, IGenericQuery query)
        {
            _accountRepository = account;
            _mapper = mapper;
            _authenticationController = authenticationController;
            _query = query;
        }

        [HttpPost]
        [Route("upload-photo")]
        public async Task<IActionResult> UploadPhoto([FromBody] Photo photo)
        {
            var account = await _authenticationController.GetLoggedInAccountId();
            await _accountRepository.UploadPhoto(photo, account);
            return Ok();
        }


        [HttpGet]
        [Route("{id:guid}/album")]
        [ResponseType(typeof(PhotosModel))]
        public async Task<IActionResult> GetProfilePhotos([FromRoute] Guid id)
        {
            string containerName = "account";
            var query = new QueryDefinition(
                query: "SELECT a.Photos FROM Account a WHERE a.id @partitionKey")
                .WithParameter("@partitionKey", id);
            PhotosModel photos = new()
            {
                Photos = await _query.GetAll<Photo>(query, containerName)
            };
            return Ok(photos);
        }
    }
}