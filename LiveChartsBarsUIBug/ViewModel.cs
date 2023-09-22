//#define USEDOUBLES

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LiveChartsBarsUIBug
{
    public partial class ViewModel : ObservableObject
    {
        private bool _isVisible;
        private ICommand _updateSeriesCommand;
        private ICommand _clearSeriesCommand;
#if USEDOUBLES
        private ObservableCollection<double> _seriesValues;
#else
        private ObservableCollection<DateTimePoint> _seriesValues;
#endif
        public ViewModel()
        {
#if USEDOUBLES
            _seriesValues = new ObservableCollection<double>();
#else
            _seriesValues = new ObservableCollection<DateTimePoint>();
#endif

            Series = new ObservableCollection<ISeries>
            {
#if USEDOUBLES
                new ColumnSeries<double>            // Works if 'LineSeries'
#else
                new ColumnSeries<DateTimePoint>     // Works if 'LineSeries'
#endif
                {
                    Values = _seriesValues
                }
            };

            PopulateSeries();
        }

        public ObservableCollection<ISeries> Series { get; set; }

        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
#if !USEDOUBLES
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks,
                Labeler = value => new DateTime((long)(value < DateTime.MinValue.Ticks ? 0 : value)).ToString("dd/MM")
#endif
            }
        };

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value != _isVisible)
                {
                    _isVisible = value;
                    OnPropertyChanged();

                    Series[0].IsVisible = _isVisible;
                }
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                if (_updateSeriesCommand == null)
                {
                    _updateSeriesCommand = new RelayCommand(PopulateSeries);
                }

                return _updateSeriesCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearSeriesCommand == null)
                {
                    _clearSeriesCommand = new RelayCommand(_seriesValues.Clear);
                }

                return _clearSeriesCommand;
            }
        }

        private void PopulateSeries()
        {
            _seriesValues.Clear();

            for (int i = 0; i < 5; i++)
            {
#if USEDOUBLES
                _seriesValues.Add(Random.Shared.NextDouble() * 100);
#else
                var date = DateTime.Now.AddDays(i);
                var value = Random.Shared.Next(100);
                _seriesValues.Add(new DateTimePoint(date, value));
#endif
            }
        }
    }
}
