// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/library.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Protos {

  /// <summary>Holder for reflection information generated from Protos/library.proto</summary>
  public static partial class LibraryReflection {

    #region Descriptor
    /// <summary>File descriptor for Protos/library.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static LibraryReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRQcm90b3MvbGlicmFyeS5wcm90bxIHbGlicmFyeSKMAQoOTGlicmFyeVJl",
            "cXVlc3QSEgoKYm9va190aXRsZRgBIAEoCRISCgpib29rX2dlbnJlGAIgASgJ",
            "EhMKC2F1dGhvcl9uYW1lGAMgASgJEhMKC3JlYWRlcl9uYW1lGAQgASgJEhMK",
            "C2JvcnJvd19kYXRlGAUgASgDEhMKC3JldHVybl9kYXRlGAYgASgDIiIKD0xp",
            "YnJhcnlSZXNwb25zZRIPCgdtZXNzYWdlGAEgASgJMlYKDkxpYnJhcnlTZXJ2",
            "aWNlEkQKDUNyZWF0ZUxpYnJhcnkSFy5saWJyYXJ5LkxpYnJhcnlSZXF1ZXN0",
            "GhgubGlicmFyeS5MaWJyYXJ5UmVzcG9uc2UoAUIJqgIGUHJvdG9zYgZwcm90",
            "bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Protos.LibraryRequest), global::Protos.LibraryRequest.Parser, new[]{ "BookTitle", "BookGenre", "AuthorName", "ReaderName", "BorrowDate", "ReturnDate" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Protos.LibraryResponse), global::Protos.LibraryResponse.Parser, new[]{ "Message" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class LibraryRequest : pb::IMessage<LibraryRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<LibraryRequest> _parser = new pb::MessageParser<LibraryRequest>(() => new LibraryRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<LibraryRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Protos.LibraryReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LibraryRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LibraryRequest(LibraryRequest other) : this() {
      bookTitle_ = other.bookTitle_;
      bookGenre_ = other.bookGenre_;
      authorName_ = other.authorName_;
      readerName_ = other.readerName_;
      borrowDate_ = other.borrowDate_;
      returnDate_ = other.returnDate_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LibraryRequest Clone() {
      return new LibraryRequest(this);
    }

    /// <summary>Field number for the "book_title" field.</summary>
    public const int BookTitleFieldNumber = 1;
    private string bookTitle_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string BookTitle {
      get { return bookTitle_; }
      set {
        bookTitle_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "book_genre" field.</summary>
    public const int BookGenreFieldNumber = 2;
    private string bookGenre_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string BookGenre {
      get { return bookGenre_; }
      set {
        bookGenre_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "author_name" field.</summary>
    public const int AuthorNameFieldNumber = 3;
    private string authorName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string AuthorName {
      get { return authorName_; }
      set {
        authorName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "reader_name" field.</summary>
    public const int ReaderNameFieldNumber = 4;
    private string readerName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string ReaderName {
      get { return readerName_; }
      set {
        readerName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "borrow_date" field.</summary>
    public const int BorrowDateFieldNumber = 5;
    private long borrowDate_;
    /// <summary>
    /// Unix timestamp in seconds
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long BorrowDate {
      get { return borrowDate_; }
      set {
        borrowDate_ = value;
      }
    }

    /// <summary>Field number for the "return_date" field.</summary>
    public const int ReturnDateFieldNumber = 6;
    private long returnDate_;
    /// <summary>
    /// Unix timestamp in seconds
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long ReturnDate {
      get { return returnDate_; }
      set {
        returnDate_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as LibraryRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(LibraryRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (BookTitle != other.BookTitle) return false;
      if (BookGenre != other.BookGenre) return false;
      if (AuthorName != other.AuthorName) return false;
      if (ReaderName != other.ReaderName) return false;
      if (BorrowDate != other.BorrowDate) return false;
      if (ReturnDate != other.ReturnDate) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (BookTitle.Length != 0) hash ^= BookTitle.GetHashCode();
      if (BookGenre.Length != 0) hash ^= BookGenre.GetHashCode();
      if (AuthorName.Length != 0) hash ^= AuthorName.GetHashCode();
      if (ReaderName.Length != 0) hash ^= ReaderName.GetHashCode();
      if (BorrowDate != 0L) hash ^= BorrowDate.GetHashCode();
      if (ReturnDate != 0L) hash ^= ReturnDate.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (BookTitle.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(BookTitle);
      }
      if (BookGenre.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(BookGenre);
      }
      if (AuthorName.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(AuthorName);
      }
      if (ReaderName.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ReaderName);
      }
      if (BorrowDate != 0L) {
        output.WriteRawTag(40);
        output.WriteInt64(BorrowDate);
      }
      if (ReturnDate != 0L) {
        output.WriteRawTag(48);
        output.WriteInt64(ReturnDate);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (BookTitle.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(BookTitle);
      }
      if (BookGenre.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(BookGenre);
      }
      if (AuthorName.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(AuthorName);
      }
      if (ReaderName.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ReaderName);
      }
      if (BorrowDate != 0L) {
        output.WriteRawTag(40);
        output.WriteInt64(BorrowDate);
      }
      if (ReturnDate != 0L) {
        output.WriteRawTag(48);
        output.WriteInt64(ReturnDate);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (BookTitle.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(BookTitle);
      }
      if (BookGenre.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(BookGenre);
      }
      if (AuthorName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AuthorName);
      }
      if (ReaderName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ReaderName);
      }
      if (BorrowDate != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(BorrowDate);
      }
      if (ReturnDate != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(ReturnDate);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(LibraryRequest other) {
      if (other == null) {
        return;
      }
      if (other.BookTitle.Length != 0) {
        BookTitle = other.BookTitle;
      }
      if (other.BookGenre.Length != 0) {
        BookGenre = other.BookGenre;
      }
      if (other.AuthorName.Length != 0) {
        AuthorName = other.AuthorName;
      }
      if (other.ReaderName.Length != 0) {
        ReaderName = other.ReaderName;
      }
      if (other.BorrowDate != 0L) {
        BorrowDate = other.BorrowDate;
      }
      if (other.ReturnDate != 0L) {
        ReturnDate = other.ReturnDate;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            BookTitle = input.ReadString();
            break;
          }
          case 18: {
            BookGenre = input.ReadString();
            break;
          }
          case 26: {
            AuthorName = input.ReadString();
            break;
          }
          case 34: {
            ReaderName = input.ReadString();
            break;
          }
          case 40: {
            BorrowDate = input.ReadInt64();
            break;
          }
          case 48: {
            ReturnDate = input.ReadInt64();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            BookTitle = input.ReadString();
            break;
          }
          case 18: {
            BookGenre = input.ReadString();
            break;
          }
          case 26: {
            AuthorName = input.ReadString();
            break;
          }
          case 34: {
            ReaderName = input.ReadString();
            break;
          }
          case 40: {
            BorrowDate = input.ReadInt64();
            break;
          }
          case 48: {
            ReturnDate = input.ReadInt64();
            break;
          }
        }
      }
    }
    #endif

  }

  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class LibraryResponse : pb::IMessage<LibraryResponse>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<LibraryResponse> _parser = new pb::MessageParser<LibraryResponse>(() => new LibraryResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<LibraryResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Protos.LibraryReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LibraryResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LibraryResponse(LibraryResponse other) : this() {
      message_ = other.message_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LibraryResponse Clone() {
      return new LibraryResponse(this);
    }

    /// <summary>Field number for the "message" field.</summary>
    public const int MessageFieldNumber = 1;
    private string message_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Message {
      get { return message_; }
      set {
        message_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as LibraryResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(LibraryResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Message != other.Message) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Message.Length != 0) hash ^= Message.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Message.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Message);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Message.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Message);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Message.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Message);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(LibraryResponse other) {
      if (other == null) {
        return;
      }
      if (other.Message.Length != 0) {
        Message = other.Message;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Message = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Message = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
