import { IMetric } from "@/interfaces/Search";

const INVALID_METRICS = [
    '',
    'Top Readership', 
    'Acceptance Rate', 
    'Article Publishing Charge',
    'Mendeley Captures',
    'Downloads',
    'Social Media',
    'Citations',
];

export function FormatMetricValue(name: string, value: number): string {
    switch (name) {
        case 'Submission to first decision':
            return `${value.toFixed(0)} days`;
        case 'Review Time': 
        case 'Publication Time':
        case 'Time to First Decision':
            return `${value.toFixed(1)} weeks`;
        case 'days to first decision for reviewed manuscripts only':
        case 'days to first decision for all manuscripts':
            return `${value.toFixed(0)}`;
        default:
            return `${value.toFixed(3)}`;
    }
}

export function CapitalizeFirstLetterOfEachWord(string: string){
    return string.replace(/\w\S*/g, function(txt){
        return txt.charAt(0).toUpperCase() + txt.substring(1).toLowerCase();
    });
}

export function FilterMetricsToShow(metrics: IMetric[]): IMetric[] {
    return metrics
                //Remove duplicated
                .filter((metric, index, self) => 
                            index === self.findIndex((n) => (n.name.toUpperCase() === metric.name.toUpperCase())))
                // Remove metrics not valid to show
                .filter(m => !INVALID_METRICS.includes(m.name))
                //Remove metrics with value 0
                .filter(m => m.value > 0);
}
