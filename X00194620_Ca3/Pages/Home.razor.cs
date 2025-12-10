using Microsoft.AspNetCore.Components;
using MudBlazor;
using X00194620_Ca3.Services;

namespace X00194620_Ca3.Pages;

public enum SortOrder{
    Ascending,
    Descending
}

public partial class Home : ComponentBase{
    [Inject] private SpaceXService SpaceX{ get; set; } = default!;
    [Inject] private NavigationManager Nav{ get; set; } = default!;

    private List<Launch>? launches;
    private bool loading = true;

    private string searchText = string.Empty;
    private string selectedStatus = "All";
    private DateTime? fromDate = null;
    private DateTime? toDate = null;

    // Advanced filters
    private bool hasPressKit = false;
    private bool hasPhotos = false;
    private bool hasWebcast = false;
    private bool hasArticle = false;
    private SortOrder sortOrder = SortOrder.Descending;
    private int currentPage = 1;
    private int pageSize = 10;
    private int TotalItems => FilteredLaunches.Count;
    private int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalItems / (double)pageSize));
    private List<Launch> PagedLaunches => FilteredLaunches.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

    private string SearchText{
        get => searchText;
        set {
            if (searchText == value) return;
            searchText = value;
            currentPage = 1;
        }
    }

    private string SelectedStatus{
        get => selectedStatus;
        set {
            if (selectedStatus == value) return;
            selectedStatus = value;
            currentPage = 1;
        }
    }

    private DateTime? FromDate{
        get => fromDate;
        set {
            if (fromDate == value) return;
            fromDate = value;
            currentPage = 1;
        }
    }

    private DateTime? ToDate{
        get => toDate;
        set {
            if (toDate == value) return;
            toDate = value;
            currentPage = 1;
        }
    }

    private bool HasPressKit{
        get => hasPressKit;
        set {
            if (hasPressKit == value) return;
            hasPressKit = value;
            currentPage = 1;
        }
    }

    private bool HasPhotos{
        get => hasPhotos;
        set {
            if (hasPhotos == value) return;
            hasPhotos = value;
            currentPage = 1;
        }
    }

    private bool HasWebcast{
        get => hasWebcast;
        set {
            if (hasWebcast == value) return;
            hasWebcast = value;
            currentPage = 1;
        }
    }

    private bool HasArticle{
        get => hasArticle;
        set {
            if (hasArticle == value) return;
            hasArticle = value;
            currentPage = 1;
        }
    }

    protected override async Task OnInitializedAsync(){
        launches = await SpaceX.GetLaunchesAsync();
        loading = false;
    }


    private void SetSortOrder(SortOrder order){
        if (sortOrder == order) return;
        sortOrder = order;
        currentPage = 1; 
        StateHasChanged();
    }

    private void ClearFilters(){
        SearchText = string.Empty;
        SelectedStatus = "All";
        FromDate = null;
        ToDate = null;
        HasPressKit = false;
        HasPhotos = false;
        HasWebcast = false;
        HasArticle = false;
        sortOrder = SortOrder.Descending;
        currentPage = 1;
    }

    private void OpenLaunch(string id){
        Nav.NavigateTo($"./launch/{id}");
    }

    private Color StatusColor(bool? success) =>
        success == true ? Color.Success :
        success == false ? Color.Error :
        Color.Warning;

    private string StatusText(bool? success) =>
        success == true ? "Success" :
        success == false ? "Failed" :
        "Unknown";

    private List<Launch> FilteredLaunches{
        get {
            if (launches is null) return new List<Launch>();

            var query = launches.AsEnumerable();

            query = ApplyBaseFilters(query);
            query = ApplyDateFilters(query);
            query = ApplyAdvancedFilters(query);
            query = ApplySorting(query);
            return query.ToList();
        }
    }


    private IEnumerable<Launch> ApplyBaseFilters(IEnumerable<Launch> query){
        query = query.Where(l => !string.IsNullOrWhiteSpace(l.Links?.Patch?.Small));

        if (!string.IsNullOrWhiteSpace(SearchText)){
            var lower = SearchText.Trim().ToLowerInvariant();
            query = query.Where(l => (l.Name ?? string.Empty).ToLowerInvariant().Contains(lower));
        }

        return SelectedStatus switch{
            "Success" => query.Where(l => l.Success == true),
            "Failed" => query.Where(l => l.Success == false),
            "Unknown" => query.Where(l => l.Success == null),
            _ => query
        };
    }

    private IEnumerable<Launch> ApplyDateFilters(IEnumerable<Launch> query){
        if (FromDate.HasValue){
            var fromDateUtc = FromDate.Value.ToUniversalTime();
            query = query.Where(l => l.DateUtc.HasValue && l.DateUtc.Value >= fromDateUtc);
        }

        if (ToDate.HasValue){
            var toDateUtc = ToDate.Value.AddDays(1).ToUniversalTime();
            query = query.Where(l => l.DateUtc.HasValue && l.DateUtc.Value < toDateUtc);
        }

        return query;
    }

    private IEnumerable<Launch> ApplyAdvancedFilters(IEnumerable<Launch> query){
        if (HasPressKit) query = query.Where(l => !string.IsNullOrWhiteSpace(l.Links?.Presskit));
        if (HasPhotos) query = query.Where(l => l.Links?.Flickr?.Original != null && l.Links.Flickr.Original.Any());
        if (HasWebcast) query = query.Where(l => !string.IsNullOrWhiteSpace(l.Links?.Webcast));
        if (HasArticle) query = query.Where(l => !string.IsNullOrWhiteSpace(l.Links?.Article));
        return query;
    }

    private IEnumerable<Launch> ApplySorting(IEnumerable<Launch> query){
        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(x => x.DateUtc ?? DateTime.MinValue)
            : query.OrderByDescending(x => x.DateUtc ?? DateTime.MaxValue);
    }
}