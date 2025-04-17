namespace logicGP.Tests.Util.ML;

public static class ColumnPropertiesHelper
{
    public static string BalanceScale = "";

    public static string Iris = """
                                [
                                  {
                                    "ColumnName": "sepal length",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "sepal width",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "petal length",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "petal width",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "class",
                                    "ColumnPurpose": "Label",
                                    "ColumnDataFormat": "String",
                                    "IsCategorical": true,
                                    "Type": "Column",
                                    "Version": 5
                                  }
                                ]
                                """;

    public static string HeartDisease = """
                                        [
                                          {
                                            "ColumnName": "age",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "sex",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "cp",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "trestbps",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "chol",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "fbs",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "restecg",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "thalach",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "exang",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "oldpeak",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "slope",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "ca",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "thal",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "num",
                                            "ColumnPurpose": "Label",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          }
                                        ]
                                        """;

    public static string WineQuality { get; set; }
    public static string BreastCancer { get; set; }
}