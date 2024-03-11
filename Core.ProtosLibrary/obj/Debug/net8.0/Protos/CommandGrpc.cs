// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/command.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Core.ProtosLibrary {
  public static partial class CommandService
  {
    static readonly string __ServiceName = "commands.CommandService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.Empty.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Core.ProtosLibrary.CommandRequest> __Marshaller_commands_CommandRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Core.ProtosLibrary.CommandRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Core.ProtosLibrary.CommandResponse> __Marshaller_commands_CommandResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Core.ProtosLibrary.CommandResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Core.ProtosLibrary.CommandResponseStatus> __Marshaller_commands_CommandResponseStatus = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Core.ProtosLibrary.CommandResponseStatus.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Core.ProtosLibrary.CommandRequest> __Method_SubscribeForCommands = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Core.ProtosLibrary.CommandRequest>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "SubscribeForCommands",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_commands_CommandRequest);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Core.ProtosLibrary.CommandResponse, global::Core.ProtosLibrary.CommandResponseStatus> __Method_StreamCommands = new grpc::Method<global::Core.ProtosLibrary.CommandResponse, global::Core.ProtosLibrary.CommandResponseStatus>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "StreamCommands",
        __Marshaller_commands_CommandResponse,
        __Marshaller_commands_CommandResponseStatus);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Core.ProtosLibrary.CommandReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of CommandService</summary>
    [grpc::BindServiceMethod(typeof(CommandService), "BindService")]
    public abstract partial class CommandServiceBase
    {
      /// <summary>
      /// rpc SendCommand(CommandRequest) returns (CommandResponse);
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="responseStream">Used for sending responses back to the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>A task indicating completion of the handler.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task SubscribeForCommands(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::IServerStreamWriter<global::Core.ProtosLibrary.CommandRequest> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// rpc InitalizeCommand(CommandInitalizeRequest) returns (CommandInitalizeResponse);
      /// </summary>
      /// <param name="requestStream">Used for reading requests from the client.</param>
      /// <param name="responseStream">Used for sending responses back to the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>A task indicating completion of the handler.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task StreamCommands(grpc::IAsyncStreamReader<global::Core.ProtosLibrary.CommandResponse> requestStream, grpc::IServerStreamWriter<global::Core.ProtosLibrary.CommandResponseStatus> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for CommandService</summary>
    public partial class CommandServiceClient : grpc::ClientBase<CommandServiceClient>
    {
      /// <summary>Creates a new client for CommandService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CommandServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for CommandService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CommandServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CommandServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CommandServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// rpc SendCommand(CommandRequest) returns (CommandResponse);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Core.ProtosLibrary.CommandRequest> SubscribeForCommands(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SubscribeForCommands(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// rpc SendCommand(CommandRequest) returns (CommandResponse);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Core.ProtosLibrary.CommandRequest> SubscribeForCommands(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_SubscribeForCommands, null, options, request);
      }
      /// <summary>
      /// rpc InitalizeCommand(CommandInitalizeRequest) returns (CommandInitalizeResponse);
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Core.ProtosLibrary.CommandResponse, global::Core.ProtosLibrary.CommandResponseStatus> StreamCommands(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return StreamCommands(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// rpc InitalizeCommand(CommandInitalizeRequest) returns (CommandInitalizeResponse);
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::Core.ProtosLibrary.CommandResponse, global::Core.ProtosLibrary.CommandResponseStatus> StreamCommands(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_StreamCommands, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override CommandServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new CommandServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(CommandServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SubscribeForCommands, serviceImpl.SubscribeForCommands)
          .AddMethod(__Method_StreamCommands, serviceImpl.StreamCommands).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, CommandServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SubscribeForCommands, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::Core.ProtosLibrary.CommandRequest>(serviceImpl.SubscribeForCommands));
      serviceBinder.AddMethod(__Method_StreamCommands, serviceImpl == null ? null : new grpc::DuplexStreamingServerMethod<global::Core.ProtosLibrary.CommandResponse, global::Core.ProtosLibrary.CommandResponseStatus>(serviceImpl.StreamCommands));
    }

  }
}
#endregion
