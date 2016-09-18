﻿namespace Microsoft.AspNetCore.Mvc.Conventions
{
    using Controllers;
    using System.Reflection;
    using Versioning;
    using Versioning.Conventions;

    public abstract class ConventionsAcceptanceTest : AcceptanceTest
    {
        protected ConventionsAcceptanceTest()
        {
            FilteredControllerTypes.Add( typeof( ValuesController ).GetTypeInfo() );
            FilteredControllerTypes.Add( typeof( Values2Controller ).GetTypeInfo() );
            FilteredControllerTypes.Add( typeof( HelloWorldController ).GetTypeInfo() );
        }

        protected override void OnAddApiVersioning( ApiVersioningOptions options )
        {
            options.ReportApiVersions = true;
            options.Conventions.Controller<ValuesController>().HasApiVersion( 1, 0 );
            options.Conventions.Controller<Values2Controller>()
                               .HasApiVersion( 2, 0 )
                               .HasApiVersion( 3, 0 )
                               .Action( c => c.GetV3() ).MapToApiVersion( 3, 0 )
                               .Action( c => c.GetV3( default( int ) ) ).MapToApiVersion( 3, 0 );
            options.Conventions.Controller<HelloWorldController>()
                               .HasApiVersion( 1, 0 )
                               .HasApiVersion( 2, 0 )
                               .AdvertisesApiVersion( 3, 0 );
        }
    }
}