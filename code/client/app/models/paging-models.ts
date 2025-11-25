
export interface PageFilter {
    index: number;
    order?: string;
    searchText?: string;
}

export interface PagedFeed<T> extends PageFilter {
    values: T[];
}
