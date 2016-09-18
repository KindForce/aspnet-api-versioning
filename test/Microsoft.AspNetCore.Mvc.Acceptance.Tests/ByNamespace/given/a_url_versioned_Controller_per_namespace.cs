﻿namespace given
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ByNamespace;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;
    using static System.Net.HttpStatusCode;

    public class _a_url_versioned_Controller_per_namespace : ByNamespaceAcceptanceTest
    {
        [Theory]
        [InlineData( "Microsoft.AspNetCore.Mvc.ByNamespace.Controllers.V1.AgreementsController", "1" )]
        [InlineData( "Microsoft.AspNetCore.Mvc.ByNamespace.Controllers.V2.AgreementsController", "2" )]
        [InlineData( "Microsoft.AspNetCore.Mvc.ByNamespace.Controllers.V3.AgreementsController", "3" )]
        public async Task _get_should_return_200( string controller, string apiVersion )
        {
            // arrange
            var example = new { controller = "", apiVersion = "", accountId = "" };

            // act
            var response = await GetAsync( $"v{apiVersion}/agreements/42" ).EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsExampleAsync( example );

            // assert
            response.Headers.GetValues( "api-supported-versions" ).Single().Should().Be( "1.0, 2.0, 3.0" );
            content.ShouldBeEquivalentTo( new { controller = controller, apiVersion = apiVersion, accountId = "42" } );
        }

        [Fact]
        public async Task _get_should_return_400_when_version_is_unsupported()
        {
            // arrange


            // act
            var response = await GetAsync( "v4/agreements/42" );
            var content = await response.Content.ReadAsAsync<OneApiErrorResponse>();

            // assert
            response.StatusCode.Should().Be( BadRequest );
            content.Error.Code.Should().Be( "UnsupportedApiVersion" );
        }
    }
}