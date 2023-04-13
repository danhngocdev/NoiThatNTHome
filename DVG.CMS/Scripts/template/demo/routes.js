angular
.module('app')
.config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', '$breadcrumbProvider', function($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, $breadcrumbProvider) {
  $stateProvider
  .state('app.icons', {
    url: "/icons",
    abstract: true,
    template: '<ui-view></ui-view>',
    ncyBreadcrumb: {
      label: 'Icons'
    }
  })
  .state('app.icons.fontawesome', {
    url: '/font-awesome',
    templateUrl: 'template/icons/font-awesome.html',
    ncyBreadcrumb: {
      label: 'Font Awesome'
    }
  })
  .state('app.icons.simplelineicons', {
    url: '/simple-line-icons',
    templateUrl: 'template/icons/simple-line-icons.html',
    ncyBreadcrumb: {
      label: 'Simple Line Icons'
    }
  })
  .state('app.components', {
    url: "/components",
    abstract: true,
    template: '<ui-view></ui-view>',
    ncyBreadcrumb: {
      label: 'Components'
    }
  })
  .state('app.components.buttons', {
    url: '/buttons',
    templateUrl: 'template/components/buttons.html',
    ncyBreadcrumb: {
      label: 'Buttons'
    }
  })
  .state('app.components.social-buttons', {
    url: '/social-buttons',
    templateUrl: 'template/components/social-buttons.html',
    ncyBreadcrumb: {
      label: 'Social Buttons'
    }
  })
  .state('app.components.cards', {
    url: '/cards',
    templateUrl: 'template/components/cards.html',
    ncyBreadcrumb: {
      label: 'Cards'
    }
  })
  .state('app.components.forms', {
    url: '/forms',
    templateUrl: 'template/components/forms.html',
    ncyBreadcrumb: {
      label: 'Forms'
    }
  })
  .state('app.components.switches', {
    url: '/switches',
    templateUrl: 'template/components/switches.html',
    ncyBreadcrumb: {
      label: 'Switches'
    }
  })
  .state('app.components.tables', {
    url: '/tables',
    templateUrl: 'template/components/tables.html',
    ncyBreadcrumb: {
      label: 'Tables'
    }
  })
  .state('app.forms', {
    url: '/forms',
    templateUrl: 'template/forms.html',
    ncyBreadcrumb: {
      label: 'Forms'
    },
    resolve: {
      loadPlugin: ['$ocLazyLoad', function ($ocLazyLoad) {
        return $ocLazyLoad.load([
          {
            serie: true,
            files: ['js/libs/moment.min.js']
          },
          {
            serie: true,
            files: ['js/libs/daterangepicker.min.js', 'js/libs/angular-daterangepicker.min.js']
          },
          {
            files: ['js/libs/mask.min.js']
          },
          {
            files: ['js/libs/select.min.js']
          }
        ]);
      }],
      loadMyCtrl: ['$ocLazyLoad', function($ocLazyLoad) {
        // you can lazy load files for an existing module
        return $ocLazyLoad.load({
          files: ['js/controllers/forms.js']
        });
      }]
    }
  })
  .state('app.widgets', {
    url: '/widgets',
    templateUrl: 'template/widgets.html',
    ncyBreadcrumb: {
      label: 'Widgets'
    },
    resolve: {
      loadMyCtrl: ['$ocLazyLoad', function($ocLazyLoad) {
        // you can lazy load controllers
        return $ocLazyLoad.load({
          files: ['js/controllers/widgets.js']
        });
      }]
    }
  })
  .state('app.charts', {
    url: '/charts',
    templateUrl: 'template/charts.html',
    ncyBreadcrumb: {
      label: 'Charts'
    },
    resolve: {
      // Plugins loaded before
      // loadPlugin: ['$ocLazyLoad', function ($ocLazyLoad) {
      //     return $ocLazyLoad.load([
      //         {
      //             serial: true,
      //             files: ['js/libs/Chart.min.js', 'js/libs/angular-chart.min.js']
      //         }
      //     ]);
      // }],
      loadMyCtrl: ['$ocLazyLoad', function($ocLazyLoad) {
        // you can lazy load files for an existing module
        return $ocLazyLoad.load({
          files: ['js/controllers/charts.js']
        });
      }]
    }
  })
}]);
