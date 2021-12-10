export interface ISearchState {
    journals: IJournal[];
    selectedJournal: IJournal;
    isLoading: boolean;
    error: string;
}

export interface IJournal {
    id: number;
    title: string;
    about: string;
    url: string;
    editorial: string;
    metrics: IMetric[];
    imgUrl: string;
}

export interface IMetric {
    id: number;
    name: string;
    value: number;
}