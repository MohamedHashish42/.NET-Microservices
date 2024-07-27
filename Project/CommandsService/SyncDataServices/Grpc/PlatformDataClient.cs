using System;
using System.Collections.Generic;
using System.Net.Http;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper,IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcPlatform"]}");
            GrpcChannel channel;
            if (_hostEnvironment.IsDevelopment())
            {
                /*
                   Configuring gRPC client to ignore SSL certificate errors. 
                   This is useful for development and testing but should not be used in production.
                */
                var httpHandler = new HttpClientHandler();
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"], new GrpcChannelOptions { HttpHandler = httpHandler });
            }
            else
            {
                channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            }

            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}