# Pinboard.net

A fully featured C# wrapper for the [pinboard.in](https://pinboard.in) API.

## Installation

    PM> Install-Package pinboard.net

## Usage

To start, retrieve the Pinboard API Token from the
[password](https://pinboard.in/settings/password) page on the website.

```
var pb = new PinboardAPI(apiToken);
```

The `pb` object can now be used to make calls to the Pinboard API

### Update

Returns the most recent time a bookmark was added, updated or deleted.

```
pb.Posts.GetLastUpdate()
```

### Posts

Add a bookmark:

```
var bookmark = new Bookmark
{
  Url = "http://linkur.co.in",
  Description = "Bookmarking for groups!",
  Extended = "",
  Tags = new List<string> { "bookmarking", "web", "tools" },
  dt = DateTime.Now,
  Shared = true,
  ToRead = false
};

pb.Posts.Add(bookmark);
```

Update a bookmark:

```
// Get the bookmark first
var bookmark = pb.Posts.Get("http://linkur.co.in").FirstOrDefault();

bookmark.Extended = "Nothing does group bookmarking better";
bookmark.Tags.Add("free");

pb.Posts.Update(bookmark)
```

Delete a bookmark:

```
pb.Posts.Delete("http://linkur.co.in");
```

Get posts matching parameters:

URL wise: 

```
pb.Posts.Get(url: "http://linkur.co.in");
```

Date wise:

```
pb.Posts.Get(date: DateTime.Now.Date);
```

Get recent bookmarks:

Filtered by tag:

```
pb.Posts.Recent(tag: new List<string> { "programming", "dotnet" });
```

Get all bookmarks:

```
pb.Posts.All()
```

### Tags

Get suggested tags for a URL:

```
pb.Posts.Suggest("https://linkur.co.in")
```

Get a full list of tags with number of times used:

```
pb.Tags.Get()
```

Delete a tag:

```
pb.Tags.Delete("prugramming");
```

Rename a tag:

```
pb.Tags.Rename("pithon", "python");
```
