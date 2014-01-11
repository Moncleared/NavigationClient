﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.7905
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IGameWcfService", CallbackContract=typeof(IGameWcfServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
public interface IGameWcfService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameWcfService/isAlive", ReplyAction="http://tempuri.org/IGameWcfService/isAliveResponse")]
    bool isAlive();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameWcfService/RegisterClient", ReplyAction="http://tempuri.org/IGameWcfService/RegisterClientResponse")]
    SharedLibrary.ClientToken RegisterClient();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameWcfService/UnregisterClient", ReplyAction="http://tempuri.org/IGameWcfService/UnregisterClientResponse")]
    void UnregisterClient(SharedLibrary.ClientToken pClientToken);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameWcfService/SendClientDetails", ReplyAction="http://tempuri.org/IGameWcfService/SendClientDetailsResponse")]
    void SendClientDetails(SharedLibrary.ClientToken pClientToken, SharedLibrary.ClientDetails pClientDetails);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IGameWcfServiceCallback
{
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameWcfService/UpdateClient")]
    void UpdateClient(SharedLibrary.ClientDetails pClientDetails);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameWcfService/IsClientAlive", ReplyAction="http://tempuri.org/IGameWcfService/IsClientAliveResponse")]
    bool IsClientAlive();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IGameWcfServiceChannel : IGameWcfService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class GameWcfServiceClient : System.ServiceModel.DuplexClientBase<IGameWcfService>, IGameWcfService
{
    
    public GameWcfServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
            base(callbackInstance)
    {
    }
    
    public GameWcfServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
            base(callbackInstance, endpointConfigurationName)
    {
    }
    
    public GameWcfServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }
    
    public GameWcfServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }
    
    public GameWcfServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, binding, remoteAddress)
    {
    }
    
    public bool isAlive()
    {
        return base.Channel.isAlive();
    }
    
    public SharedLibrary.ClientToken RegisterClient()
    {
        return base.Channel.RegisterClient();
    }
    
    public void UnregisterClient(SharedLibrary.ClientToken pClientToken)
    {
        base.Channel.UnregisterClient(pClientToken);
    }
    
    public void SendClientDetails(SharedLibrary.ClientToken pClientToken, SharedLibrary.ClientDetails pClientDetails)
    {
        base.Channel.SendClientDetails(pClientToken, pClientDetails);
    }
}
