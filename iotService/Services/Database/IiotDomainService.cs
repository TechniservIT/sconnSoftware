using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Data.Entity.Core.Objects;

namespace iotServiceProvider
{
    public class ApplyDataContractResolverAttribute : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {

        }

        public void ApplyClientBehavior(OperationDescription description, System.ServiceModel.Dispatcher.ClientOperation proxy)
        {
            var dataContractSerializerOperationBehavior =
                description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            dataContractSerializerOperationBehavior.DataContractResolver =
                new ProxyDataContractResolver();
        }

        public void ApplyDispatchBehavior(OperationDescription description, System.ServiceModel.Dispatcher.DispatchOperation dispatch)
        {
            var dataContractSerializerOperationBehavior =
                description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            dataContractSerializerOperationBehavior.DataContractResolver =
                new ProxyDataContractResolver();
        }

        public void Validate(OperationDescription description)
        {
            // Do validation.
        }
    }


    [ServiceContract]
    public interface IiotDomainService
    {

        //  [ApplyDataContractResolver]

        /********************* Add *********************/
        [OperationContract][ApplyDataContractResolver]
        bool DomainAdd(iotDomain domain);
        
        [OperationContract][ApplyDataContractResolver]
        bool DeviceAdd(Device domain);

        [OperationContract][ApplyDataContractResolver]
        bool SiteAdd(Site domain);

        [OperationContract][ApplyDataContractResolver]
        bool ActionAdd(DeviceAction domain);

        [OperationContract][ApplyDataContractResolver]
        bool PropertyAdd(DeviceProperty domain);

        [OperationContract][ApplyDataContractResolver]
        bool LocationAdd(Location domain);

        [OperationContract][ApplyDataContractResolver]
        bool ResParamAdd(DeviceParameter domain);

        [OperationContract][ApplyDataContractResolver]
        bool ReqParamAdd(ActionParameter domain);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceTypeAdd(DeviceType type);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceCredentialsAdd(DeviceCredentials creds);

        [OperationContract][ApplyDataContractResolver]
        bool EndpointAdd(EndpointInfo endp);



        /********************* Get *********************/
        [OperationContract][ApplyDataContractResolver]
        List<Site> Sites();

        [OperationContract][ApplyDataContractResolver]
        List<Device> Devices();

        [OperationContract][ApplyDataContractResolver]
        List<DeviceAction> DeviceActions();

        [OperationContract][ApplyDataContractResolver]
        List<DeviceProperty> DeviceProperties();

        [OperationContract][ApplyDataContractResolver]
        List<DeviceParameter> DeviceParameters();

        [OperationContract][ApplyDataContractResolver]
        List<iotDomain> Domains();

        [OperationContract][ApplyDataContractResolver]
        iotDomain GetDomainWithName(string name);


        [OperationContract][ApplyDataContractResolver]
        List<Location> Locations();

        [OperationContract][ApplyDataContractResolver]
        List<DeviceType> DeviceTypes();

        [OperationContract][ApplyDataContractResolver]
        List<ActionParameter> ActionParams();

        [OperationContract][ApplyDataContractResolver]
        List<DeviceCredentials> DeviceCredentials();

        [OperationContract][ApplyDataContractResolver]
        List<EndpointInfo> Endpoints();



        /*************************** Get By Id ****************************/

        [OperationContract][ApplyDataContractResolver]
        iotDomain GetDomainWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        Site SiteWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        Device DeviceWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        DeviceAction DeviceActionWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        DeviceProperty DevicePropertieWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        DeviceParameter DeviceParameterWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        iotDomain DomainWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        Location LocationWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        DeviceType DeviceTypeWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        ActionParameter ActionParamWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        DeviceCredentials DeviceCredentialWithId(int id);

        [OperationContract][ApplyDataContractResolver]
        
        EndpointInfo EndpointWithId(int id);



        /*********************** Update ***********************/

        [OperationContract][ApplyDataContractResolver]
        bool DomainUpdate(iotDomain domain);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceUpdate(Device domain);

        [OperationContract][ApplyDataContractResolver]
        bool SiteUpdate(Site domain);

        [OperationContract][ApplyDataContractResolver]
        bool ActionUpdate(DeviceAction domain);

        [OperationContract][ApplyDataContractResolver]
        bool PropertyUpdate(DeviceProperty domain);

        [OperationContract][ApplyDataContractResolver]
        bool LocationUpdate(Location domain);

        [OperationContract][ApplyDataContractResolver]
        bool ResParamUpdate(DeviceParameter domain);

        [OperationContract][ApplyDataContractResolver]
        bool ReqParamUpdate(ActionParameter domain);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceTypeUpdate(DeviceType type);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceCredentialsUpdate(DeviceCredentials creds);

        [OperationContract][ApplyDataContractResolver]
        bool EndpointUpdate(EndpointInfo endp);


        /*********************** Delete ***********************/
        [OperationContract][ApplyDataContractResolver]
        bool DomainRemove(iotDomain domain);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceRemove(Device domain);

        [OperationContract][ApplyDataContractResolver]
        bool SiteRemove(Site domain);

        [OperationContract][ApplyDataContractResolver]
        bool ActionRemove(DeviceAction domain);

        [OperationContract][ApplyDataContractResolver]
        bool PropertyRemove(DeviceProperty domain);

        [OperationContract][ApplyDataContractResolver]
        bool LocationRemove(Location domain);

        [OperationContract][ApplyDataContractResolver]
        bool ResParamRemove(DeviceParameter domain);

        [OperationContract][ApplyDataContractResolver]
        bool ReqParamRemove(ActionParameter domain);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceTypeRemove(DeviceType type);

        [OperationContract][ApplyDataContractResolver]
        bool DeviceCredentialsRemove(DeviceCredentials creds);

        [OperationContract][ApplyDataContractResolver]
        bool EndpointRemove(EndpointInfo endp);




        /*********************************   IOT  QUERY / UPDATE    ********************************/
        [OperationContract][ApplyDataContractResolver]
        bool UpdateDeviceProperties(Device dev);

        [OperationContract][ApplyDataContractResolver]
        bool  UpdateDeviceActionState(Device dev);

        [OperationContract][ApplyDataContractResolver]
        bool PerformDeviceAction(DeviceAction act);



        /************************  CUSTOM CROSS QUERY ********************/
        [OperationContract]
        [ApplyDataContractResolver]
        Device DeviceWithEndpoint(EndpointInfo endp);



    }



  



    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "iotServiceProvider.ContractType".

}
