
namespace SM.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }  
        public List<Comment> Replies { get; set; } = [];
    }

    //DATABASE SCHEMA {
//  "id": "comment-123",
//  "postId": "post-789",         // Link to the original post
//  "author": "user123",
//  "content": "This is a top-level comment.",
//  "createdAt": "2025-05-19T14:30:00Z",
//  "replies": [
//    {
//      "id": "comment-124",
//      "author": "user456",
//      "content": "This is a reply.",
//      "createdAt": "2025-05-19T14:35:00Z",
//      "replies": [
//        {
//          "id": "comment-125",
//          "author": "user789",
//          "content": "This is a nested reply.",
//          "createdAt": "2025-05-19T14:40:00Z",
//          "replies": []
//}
//      ]
//    },
//    {
//    "id": "comment-126",
//      "author": "user111",
//      "content": "Another first-level reply.",
//      "createdAt": "2025-05-19T14:36:00Z",
//      "replies": []
//    }
//  ]
//}

}
